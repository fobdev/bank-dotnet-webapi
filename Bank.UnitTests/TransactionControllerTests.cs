using System;
using Bank.Api.Controllers;
using Bank.Api.Repositories;
using Bank.Api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;


namespace Banking.UnitTests
{
    public class TransactionControllerTests
    {

        private readonly Mock<ITransactionRepository> transactionRepositoryStub = new();
        private readonly Mock<IUserRepository> userRepositoryStub = new();
        private readonly Random randomNumber = new();

        [Fact]
        public void GetTransactionById_WhenTransactionNotExists_ShouldReturnNotFound()
        {
            // Arrange
            transactionRepositoryStub.Setup(repo => repo.GetTransactionById(It.IsAny<Guid>()))
                .Returns((Transaction)null);

            var controller = new TransactionController(transactionRepositoryStub.Object, userRepositoryStub.Object);

            // Act
            var result = controller.GetTransactionById(Guid.NewGuid());

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetTransactionById_WhenTransactionExists_ShouldReturnTransaction()
        {
            // Arrange
            Transaction expectedItem = CreateRandomTransaction();

            transactionRepositoryStub.Setup(repo => repo.GetTransactionById(It.IsAny<Guid>()))
                .Returns(expectedItem);

            var controller = new TransactionController(transactionRepositoryStub.Object, userRepositoryStub.Object);

            // Act
            var result = controller.GetTransactionById(Guid.NewGuid());

            // Assert
            result.Value.Should().BeEquivalentTo(expectedItem,
            options => options.ComparingByMembers<Transaction>().ExcludingMissingMembers());
        }

        [Fact]
        public void GetAllTransactions_WhenExists_ShouldReturnAllTransactions()
        {
            // Arrange
            var expectedTransactions = new[] { CreateRandomTransaction(), CreateRandomTransaction(), CreateRandomTransaction() };

            transactionRepositoryStub.Setup(repo => repo.GetTransactions())
                .Returns(expectedTransactions);

            var controller = new TransactionController(transactionRepositoryStub.Object, userRepositoryStub.Object);

            // Act
            var result = controller.GetAllTransactions();

            // Assert
            result.Should().BeEquivalentTo(expectedTransactions,
            options => options.ComparingByMembers<Transaction>().ExcludingMissingMembers());
        }

        [Fact]
        public void CreateTransaction_WithTransactionToCreate_ShouldReturnCreatedTransaction()
        {
            // Arrange
            var transactionToCreate = new TransactionCreateDto()
            {
                amount = randomNumber.Next(),
                sender = Guid.NewGuid(),
                receiver = Guid.NewGuid(),
            };
            var controller = new TransactionController(transactionRepositoryStub.Object, userRepositoryStub.Object);

            // Act
            var result = controller.CreateTransaction(transactionToCreate);

            // Assert
            var createdTransaction = (result.Result as CreatedAtActionResult).Value as TransactionDto;
            createdTransaction.id.Should().NotBeEmpty();
            transactionToCreate.Should().BeEquivalentTo(
                createdTransaction,
                options => options.ComparingByMembers<TransactionDto>().ExcludingMissingMembers()
            );
        }
        [Fact]
        public void RevertTransaction_WhenTransactionExists_ShouldReturnNoContent()
        {
            // Arrange
            var newTransactionToCreate = CreateRandomTransaction();

            transactionRepositoryStub.Setup(repo => repo.GetTransactionById(It.IsAny<Guid>()))
                        .Returns(newTransactionToCreate);

            var controller = new TransactionController(transactionRepositoryStub.Object, userRepositoryStub.Object);

            // Act
            var result = controller.RevertTransaction(newTransactionToCreate.id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        private Transaction CreateRandomTransaction()
        {
            return new()
            {
                id = Guid.NewGuid(),
                receiver = Guid.NewGuid(),
                sender = Guid.NewGuid(),
                amount = randomNumber.NextDouble(),
                createdAt = DateTimeOffset.UtcNow
            };
        }
    }
}