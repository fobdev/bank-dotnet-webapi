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
        private readonly IUserRepository _users_repository;
        public TransactionController(ITransactionRepository transactions_repository, IUserRepository users_repository)
        {
            this._transactions_repository = transactions_repository;
            this._users_repository = users_repository;
        }

        // [GET] endpoint: transactions/
        [HttpGet]
        public IEnumerable<TransactionDto> GetAllTransactions()
        {
            var transactions = _transactions_repository.GetTransactions().Select(item => item.AsTransactionDto());
            return transactions;
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
        public ActionResult CreateTransaction([FromBody] TransactionCreateDto transactionDto)
        {
            var existingSender = _users_repository.GetUserById(transactionDto.sender);
            if (existingSender is null) return NotFound();

            var existingReceiver = _users_repository.GetUserById(transactionDto.receiver);
            if (existingReceiver is null) return NotFound();

            // check conflicts
            if (existingSender.staff)
                return BadRequest("Staff users can only receive transactions.");
            if (existingSender.balance < transactionDto.amount)
                return BadRequest("The transaction amount is larger than the current sender balance.");
            if (transactionDto.amount <= 0)
                return BadRequest("Please input a valid transaction number.");

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