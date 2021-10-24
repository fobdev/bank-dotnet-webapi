using System;
using Bank.Api.Controllers;
using Bank.Api.Dtos;
using Bank.Api.Models;
using Bank.Api.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Bank.UnitTests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserRepository> userRepositoryMock = new();

        private readonly Random randomNumber = new();


        [Fact]
        public void GetUserById_WhenUserNotExists_ShouldReturnNotFound()
        {
            // Arrange
            userRepositoryMock.Setup(repo => repo.GetUserById(It.IsAny<Guid>()))
                .Returns((User)null);

            var controller = new UserController(userRepositoryMock.Object);

            // Act
            var result = controller.GetUserById(Guid.NewGuid());

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetUserById_WhenExists_ShouldReturnExpectedUser()
        {
            // Arrange
            User expectedItem = CreateRandomUser();

            userRepositoryMock.Setup(repo => repo.GetUserById(It.IsAny<Guid>()))
                .Returns(expectedItem);

            var controller = new UserController(userRepositoryMock.Object);

            // Act
            var result = controller.GetUserById(Guid.NewGuid());

            // Assert
            result.Value.Should().BeEquivalentTo(expectedItem,
            options => options.ComparingByMembers<User>().ExcludingMissingMembers());
        }

        [Fact]
        public void GetUsers_WhenExists_ShouldReturnAllUsers()
        {
            // Arrange
            var expectedUsers = new[] { CreateRandomUser(), CreateRandomUser(), CreateRandomUser() };

            userRepositoryMock.Setup(repo => repo.GetUsers())
                .Returns(expectedUsers);

            var controller = new UserController(userRepositoryMock.Object);

            // Act
            var result = controller.GetUsers();

            // Assert
            result.Should().BeEquivalentTo(expectedUsers,
            options => options.ComparingByMembers<User>().ExcludingMissingMembers());
        }

        [Fact]
        public void CreateUser_WithUserToCreate_ShouldReturnCreatedUser()
        {
            // Arrange
            var userToCreate = new UserCreateDto()
            {
                name = Guid.NewGuid().ToString(),
                cpf = Guid.NewGuid().ToString(),
                email = Guid.NewGuid().ToString(),
                password = Guid.NewGuid().ToString(),
                staff = false,
            };

            var controller = new UserController(userRepositoryMock.Object);

            // Act
            var result = controller.CreateUser(userToCreate);

            // Assert
            var createdUser = (result.Result as CreatedAtActionResult).Value as UserDto;
            createdUser.id.Should().NotBeEmpty();
            userToCreate.Should().BeEquivalentTo(
                createdUser,
                options => options.ComparingByMembers<UserDto>().ExcludingMissingMembers()
            );
        }
        private User CreateRandomUser()
        {
            return new()
            {
                id = Guid.NewGuid(),
                name = Guid.NewGuid().ToString(),
                cpf = Guid.NewGuid().ToString(),
                email = Guid.NewGuid().ToString(),
                password = Guid.NewGuid().ToString(),
                balance = 9500,
                staff = true,
            };
        }
    }
}
