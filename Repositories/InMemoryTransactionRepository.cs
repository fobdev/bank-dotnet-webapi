using System;
using System.Collections.Generic;
using System.Linq;
using Bank.Models;

namespace Bank.Repositories
{
    public class InMemoryTransactionRepository : ITransactionsRepository
    {

        private readonly IUserRepository user_repository;

        public InMemoryTransactionRepository(IUserRepository user_repository)
        {
            this.user_repository = user_repository;
        }

        private readonly List<Transaction> transactions = new() { };

        public Transaction GetTransactionById(Guid id)
        {
            return transactions.Where(transaction => transaction.id == id).SingleOrDefault();
        }

        public IEnumerable<Transaction> GetTransactions()
        {
            return transactions;
        }

        public void MakeTransaction(User sender, User receiver, double amount)
        {
            throw new NotImplementedException();
        }

        public void RevertTransaction(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}