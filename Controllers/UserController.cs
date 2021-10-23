using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Bank.Dtos;
using Bank.Models;
using Bank.Repositories;
using Microsoft.AspNetCore.Mvc;

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
            User user = new()
            {
                id = Guid.NewGuid(),
                name = userDto.name,
                email = userDto.email,
                cpf = userDto.cpf,
                password = userDto.password,
                type = userDto.type,
                balance = 0,
            };

            _user_repository.CreateUser(user);

            return CreatedAtAction(nameof(GetUserById), new { id = user.id }, user.AsUserDto());
        }
    }
}