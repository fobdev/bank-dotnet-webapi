using System;
using System.Collections.Generic;
using Bank.Models;

namespace Bank.Repositories
{

    public interface ITransactionRepository
    {
        IEnumerable<Transaction> GetTransactions();

        void MakeTransaction(User sender, User receiver, double amount);

        void RevertTransaction(Guid id);

        Transaction GetTransactionById(Guid id);
    }
}