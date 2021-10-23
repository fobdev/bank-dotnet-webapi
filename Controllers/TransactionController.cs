using System;
using System.Collections.Generic;
using System.Linq;
using Bank.Repositories;
using Bank.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Controllers
{

    // Route: host:port/transactions/
    [ApiController]
    [Route("transactions")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository transaction_repository;
        private readonly IUserRepository user_repository;
        public TransactionController(ITransactionRepository transactions_repository, IUserRepository user_repository)
        {
            this.transaction_repository = transactions_repository;
            this.user_repository = user_repository;
        }

        // [GET] endpoint: transactions/
        [HttpGet]
        public ActionResult<IEnumerable<TransactionDto>> GetAllTransactions()
        {
            var transactions = transaction_repository.GetTransactions().Select(item => item.AsTransactionDto());
            return Ok(transactions);
        }


        // [GET] endpoint: transactions/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<TransactionDto> GetTransactionById(Guid id)
        {
            var transaction = transaction_repository.GetTransactionById(id);
            if (transaction is null) return NotFound();

            return Ok(transaction.AsTransactionDto());
        }

        // [POST] endpoint: transactions/create
        [HttpPost("create")]
        public ActionResult MakeTransaction([FromBody] TransactionCreateDto transaction)
        {
            var existingSender = user_repository.GetUser(transaction.sender);
            if (existingSender is null) return NotFound();

            var existingReceiver = user_repository.GetUser(transaction.receiver);
            if (existingReceiver is null) return NotFound();

            // sales user type don't send money, only receive
            if (existingSender.type == "sales") return Forbid();
            if (existingSender.balance < transaction.amount) return Forbid();

            transaction_repository.CreateTransaction(existingSender, existingReceiver, transaction.amount);

            return NoContent();
        }

        // [POST] endpoint: transactions/undotransaction/{id}
        [HttpPost("undotransaction/{id:guid}")]
        public ActionResult RevertTransaction(Guid id)
        {
            var existingTransaction = transaction_repository.GetTransactionById(id);
            if (existingTransaction is null) return NotFound();

            transaction_repository.RevertTransaction(id);

            return NoContent();
        }

    }
}