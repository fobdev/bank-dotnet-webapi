using System;
using System.Collections.Generic;
using Bank.Models;
using MongoDB.Driver;

namespace Bank.Repositories
{
    class MongoDBTransactionsRepository : ITransactionRepository
    {
        private const string databaseName = "bank";
        private const string collectionName = "transactions";
        private readonly IMongoCollection<Transaction> transactionCollection;
        private readonly IUserRepository _user_repository;

        public MongoDBTransactionsRepository(IMongoClient mongoClient, IUserRepository user_repository)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            transactionCollection = database.GetCollection<Transaction>(collectionName);

            this._user_repository = user_repository;

        }
        public void CreateTransaction(User sender, User receiver, double amount)
        {
            var newSenderBalance = sender.balance - amount;
            var newReceiverBalance = receiver.balance + amount;

            _user_repository.SetUserBalance(_user_repository.GetUser(sender.id), newSenderBalance);
            _user_repository.SetUserBalance(_user_repository.GetUser(receiver.id), newReceiverBalance);

            Transaction transaction = new()
            {
                id = Guid.NewGuid(),
                amount = amount,
                sender = sender.id,
                receiver = receiver.id,
                createdAt = DateTimeOffset.UtcNow
            };

            transactionCollection.InsertOne(transaction);
        }
        public IEnumerable<Transaction> GetTransactions()
        {
            throw new NotImplementedException();
        }
        public Transaction GetTransactionById(Guid id)
        {
            throw new NotImplementedException();
        }
        public void RevertTransaction(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}