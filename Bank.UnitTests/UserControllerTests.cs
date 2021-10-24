using System;
using Bank.Api.Controllers;
using Bank.Api.Dtos;
using Bank.Api.Models;
using Bank.Api.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Bank.Api;

namespace Bank.UnitTests
{
    public class UserControllerTests
    {
        private readonly Mock<IUserRepository> userRepositoryStub = new();

        private readonly Random randomNumber = new();


        [Fact]
        public void GetUserById_WhenUserNotExists_ShouldReturnNotFound()
        {
            // Arrange
            userRepositoryStub.Setup(repo => repo.GetUserById(It.IsAny<Guid>()))
                .Returns((User)null);

            var controller = new UserController(userRepositoryStub.Object);

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

            userRepositoryStub.Setup(repo => repo.GetUserById(It.IsAny<Guid>()))
                .Returns(expectedItem);

            var controller = new UserController(userRepositoryStub.Object);

            // Act
            var result = controller.GetUserById(Guid.NewGuid());

            // Assert
            result.Value.Should().BeEquivalentTo(expectedItem.AsUserDto(),
            options => options.ComparingByMembers<UserDto>());
        }

        [Fact]
        public void GetUsers_WhenExists_ShouldReturnAllUsers()
        {
            // Arrange
            var expectedUsers = new[] { CreateRandomUser(), CreateRandomUser(), CreateRandomUser() };
            userRepositoryStub.Setup(repo => repo.GetUsers())
                .Returns(expectedUsers);

            var controller = new UserController(userRepositoryStub.Object);

            // Act
            var result = controller.GetUsers();

            // Assert
            result.Should().BeEquivalentTo(expectedUsers,
            options => options.ComparingByMembers<UserDto>());
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
