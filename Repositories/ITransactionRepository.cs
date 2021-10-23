using System;
using System.Collections.Generic;
using Bank.Models;

namespace Bank.Repositories
{

    public interface ITransactionRepository
    {
        IEnumerable<Transaction> GetTransactions();
        Transaction GetTransactionById(Guid id);
        void CreateTransaction(User sender, User receiver, double amount);
        void RevertTransaction(Guid id);
    }
}