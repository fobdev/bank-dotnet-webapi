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
        public ActionResult<IEnumerable<TransactionDto>> GetAllTransactions()
        {
            var transactions = _transactions_repository.GetTransactions().Select(item => item.AsTransactionDto());
            return Ok(transactions);
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
        public ActionResult CreateTransaction([FromBody] TransactionCreateDto transaction)
        {
            var existingSender = _users_repository.GetUserById(transaction.sender);
            if (existingSender is null) return NotFound();

            var existingReceiver = _users_repository.GetUserById(transaction.receiver);
            if (existingReceiver is null) return NotFound();

            // logic check
            if (existingSender.staff)
                return BadRequest("Staff users can only receive transactions.");
            if (existingSender.balance < transaction.amount)
                return BadRequest("The transaction amount is larger than the current sender balance.");
            if (transaction.amount <= 0)
                return BadRequest("Please input a valid transaction number.");

            _transactions_repository.CreateTransaction(existingSender, existingReceiver, transaction.amount);

            return NoContent();
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