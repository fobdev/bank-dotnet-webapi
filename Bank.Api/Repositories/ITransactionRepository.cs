using System;
using System.Collections.Generic;
using Bank.Api.Models;

namespace Bank.Api.Repositories
{

    public interface ITransactionRepository
    {
        IEnumerable<Transaction> GetTransactions();
        Transaction GetTransactionById(Guid id);
        void CreateTransaction(Transaction transaction);
        void RevertTransaction(Guid id);
        bool CheckUserIsStaff(Guid id);
        bool CheckTransactionSenderBalance(TransactionCreateDto transaction);
    }
}