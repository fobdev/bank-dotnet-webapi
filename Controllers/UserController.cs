using System;
using System.Collections.Generic;
using System.Linq;
using Bank.Dtos;
using Bank.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Controllers
{
    [ApiController]
    [Route("user")]
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
    }
}