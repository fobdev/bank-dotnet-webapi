using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Bank.Dtos;
using Bank.Models;
using Bank.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Bank.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _user_repository;
        public UserController(IUserRepository user_repository)
        {
            this._user_repository = user_repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetUsers()
        {
            var users = _user_repository.GetUsers().Select(user => user.AsUserDto());
            return Ok(users);
        }

        // [GET] endpoint: users/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<UserDto> GetUserById(Guid id)
        {
            var user = _user_repository.GetUser(id);
            if (user is null) return NotFound();
            return Ok(user.AsUserDto());
        }
        [HttpPost]
        public ActionResult<UserDto> CreateUser(UserCreateDto userDto)
        {
            // password hashing
            byte[] specialsugar = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
                rngCsp.GetNonZeroBytes(specialsugar);

            string hashedpwd = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: userDto.password,
            salt: specialsugar,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 99999,
            numBytesRequested: 256 / 8));

            // check duplicates
            if (_user_repository.ExistsCPF(userDto.cpf) || _user_repository.ExistsEmail(userDto.email))
                return StatusCode(403);

            // user creation
            User user = new()
            {
                id = Guid.NewGuid(),
                name = userDto.name,
                email = userDto.email,
                cpf = userDto.cpf,
                password = hashedpwd,
                staff = userDto.staff,
                balance = 0,
            };

            _user_repository.CreateUser(user);

            return CreatedAtAction(nameof(GetUserById), new { id = user.id }, user.AsUserDto());
        }
    }
}