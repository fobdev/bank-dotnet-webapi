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
    public class BankControllerTests
    {
        private readonly Mock<IBankRepository> bankRepositoryMock = new();

        [Fact]
        public void GetBankById_WhenBankDoesNotExists_ShouldReturnNotFound()
        {
            // Arrange
            bankRepositoryMock.Setup(repo => repo.GetBankById(It.IsAny<Guid>()))
                .Returns((PhysicalBank)null);

            var controller = new BankController(bankRepositoryMock.Object);

            // Act
            var result = controller.GetBankById(Guid.NewGuid());

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetBankById_WhenExists_ShouldReturnExpectedBank()
        {
            // Arrange
            PhysicalBank expectedItem = CreateRandomBank();

            bankRepositoryMock.Setup(repo => repo.GetBankById(It.IsAny<Guid>()))
                .Returns(expectedItem);

            var controller = new BankController(bankRepositoryMock.Object);

            // Act
            var result = controller.GetBankById(Guid.NewGuid());

            // Assert
            result.Value.Should().BeEquivalentTo(expectedItem,
            options => options.ComparingByMembers<PhysicalBank>().ExcludingMissingMembers());
        }

        [Fact]
        public void CreateBank_WithBankToCreate_ShouldReturnCreatedBank()
        {
            // Arrange
            var bankToCreate = new PhysicalBankCreateDto()
            {
                balance = 100000
            };

            var controller = new BankController(bankRepositoryMock.Object);

            // Act
            var result = controller.CreateBank(bankToCreate);

            // Assert
            var createdBank = (result.Result as CreatedAtActionResult).Value as PhysicalBankDto;
            bankToCreate.Should().BeEquivalentTo(
                createdBank,
                options => options.ComparingByMembers<PhysicalBankDto>().ExcludingMissingMembers()
            );
        }

        private PhysicalBank CreateRandomBank()
        {
            return new()
            {
                id = Guid.NewGuid(),
                balance = new Random().NextDouble(),
            };
        }

    }
}
