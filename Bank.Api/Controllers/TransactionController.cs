using System;
using System.Collections.Generic;
using System.Linq;
using Bank.Api.Repositories;
using Bank.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Controllers
{

    // Route: host:port/transactions/
    [ApiController]
    [Route("transactions")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _transactions_repository;
        public TransactionController(ITransactionRepository transactions_repository, IUserRepository users_repository)
        {
            this._transactions_repository = transactions_repository;
        }

        // [GET] endpoint: transactions/
        [HttpGet]
        public IEnumerable<TransactionDto> GetAllTransactions()
        {
            return _transactions_repository.GetTransactions().Select(item => item.AsTransactionDto());
        }

        // [GET] endpoint: transactions/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<TransactionDto> GetTransactionById(Guid id)
        {
            var transaction = _transactions_repository.GetTransactionById(id);
            if (transaction is null) return NotFound();

            return transaction.AsTransactionDto();
        }

        // [POST] endpoint: transactions/create
        [HttpPost("create")]
        public ActionResult<TransactionDto> CreateTransaction(TransactionCreateDto transactionDto)
        {
            // check conflicts
            if (transactionDto.sender == transactionDto.receiver)
                return BadRequest("Transaction needs to be from two different users.");
            if (transactionDto.amount <= 0)
                return BadRequest("Please input a valid transaction number.");
            if (_transactions_repository.CheckUserIsStaff(transactionDto.sender))
                return BadRequest("Staff members can only receive money.");
            if (_transactions_repository.CheckTransactionSenderBalance(transactionDto))
                return BadRequest("The transaction value must be smaller or equal to the user balance.");

            Transaction transaction = new()
            {
                id = Guid.NewGuid(),
                amount = transactionDto.amount,
                sender = transactionDto.sender,
                receiver = transactionDto.receiver,
                createdAt = DateTimeOffset.UtcNow
            };

            _transactions_repository.CreateTransaction(transaction);

            return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.id }, transaction.AsTransactionDto());
        }

        // [POST] endpoint: transactions/undotransaction/{id}
        [HttpPost("undotransaction/{id:guid}")]
        public ActionResult RevertTransaction(Guid id)
        {
            var existingTransaction = _transactions_repository.GetTransactionById(id);
            if (existingTransaction is null) return NotFound();

            _transactions_repository.RevertTransaction(id);

            return NoContent();
        }

    }
}