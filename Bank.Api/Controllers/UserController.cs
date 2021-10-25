using System;
using System.Collections.Generic;
using System.Linq;
using Bank.Api.Dtos;
using Bank.Api.Models;
using Bank.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Net.Mail;

namespace Bank.Api.Controllers
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
        public IEnumerable<UserDto> GetUsers()
        {
            var users = _user_repository.GetUsers()
                    .Select(user => user.AsUserDto());

            return users;
        }

        // [GET] endpoint: users/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<UserDto> GetUserById(Guid id)
        {
            var user = _user_repository.GetUserById(id);
            if (user is null) return NotFound();

            return user.AsUserDto();
        }
        [HttpPost]
        public ActionResult<UserDto> CreateUser(UserCreateDto userDto)
        {
            // password hashing
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
                rngCsp.GetNonZeroBytes(salt);

            string hashedpwd = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: userDto.password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 99999,
            numBytesRequested: 256 / 8));

            // check conflicts and bad requests
            if (_user_repository.ExistsEmail(userDto.email) || _user_repository.ExistsCPF(userDto.cpf))
                return Conflict("The Email or CPF already exists in the database.");

            if (!MailAddress.TryCreate(userDto.email, out MailAddress mailValidation))
                return BadRequest($"{userDto.email} is invalid, please use this format: xxx@xxx.xxx");

            // TODO Verification for valid CPF format

            // TODO Send confirmation mail to validated Email

            // user creation
            User user = new()
            {
                id = Guid.NewGuid(),
                name = userDto.name,
                email = userDto.email,
                cpf = userDto.cpf,
                password = hashedpwd,
                staff = userDto.staff,
                balance = 1000,
            };

            _user_repository.CreateUser(user);

            return CreatedAtAction(nameof(GetUserById), new { id = user.id }, user.AsUserDto());
        }
    }
}