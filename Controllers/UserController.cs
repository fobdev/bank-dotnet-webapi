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
        private readonly IUserRepository user_repository;
        public UserController(IUserRepository user_repository)
        {
            this.user_repository = user_repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> GetUsers()
        {
            var users = user_repository.GetUsers().Select(user => user.AsUserDto());
            return Ok(users);
        }

        // [GET] endpoint: users/{id}
        [HttpGet("{id}")]
        public ActionResult<UserDto> GetUserById(Guid id)
        {
            var user = user_repository.GetUser(id);
            if (user is null) return NotFound();
            return Ok(user.AsUserDto());
        }
        [HttpPost]
        public ActionResult<UserDto> CreateUser(UserCreateDto userDto)
        {
            List<User> userList = user_repository.GetUsers().ToList<User>();

            if (userList.Find(user => user.email == userDto.email) is not null ||
                userList.Find(user => user.cpf == userDto.cpf) is not null)
                return Forbid();

            // Format validations
            string cpfcnpjRegexString = @"/(^\d{3}\.\d{3}\.\d{3}\-\d{2}$)|(^\d{2}\.\d{3}\.\d{3}\/\d{4}\-\d{2}$)/";
            if (Regex.IsMatch(userDto.email, cpfcnpjRegexString))
                return Forbid();

            MailAddress validateMail;
            try
            {
                validateMail = new MailAddress(userDto.email);
            }
            catch (FormatException)
            {
                return Forbid();
            }

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

            user_repository.CreateUser(user);

            return CreatedAtAction(nameof(GetUserById), new { id = user.id }, user.AsUserDto());
        }
    }
}