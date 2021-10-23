using System;
using System.Collections.Generic;
using Bank.Models;

namespace Banking.Repositories
{

    public interface ITransactionsRepository
    {
        IEnumerable<Transaction> GetTransactions();

        void MakeTransaction(User sender, User receiver, double amount);

        void RevertTransaction(Guid id);

        Transaction GetTransactionById(Guid id);
    }
}