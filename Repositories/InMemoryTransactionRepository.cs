using System;
using System.Collections.Generic;
using System.Linq;
using Bank.Models;

namespace Bank.Repositories
{
    public class InMemoryTransactionRepository : ITransactionRepository
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

        public void CreateTransaction(User sender, User receiver, double amount)
        {
            var newSenderBalance = sender.balance - amount;
            var newReceiverBalance = receiver.balance + amount;

            user_repository.SetUserBalance(user_repository.GetUser(sender.id), newSenderBalance);
            user_repository.SetUserBalance(user_repository.GetUser(receiver.id), newReceiverBalance);

            transactions.Add(new Transaction
            {
                id = Guid.NewGuid(),
                amount = amount,
                sender = sender.id,
                receiver = receiver.id,
                createdAt = DateTimeOffset.UtcNow
            });
        }

        public void RevertTransaction(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}