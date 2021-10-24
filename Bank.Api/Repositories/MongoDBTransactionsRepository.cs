using System;
using System.Collections.Generic;
using Bank.Api.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Bank.Api.Repositories
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
        public void CreateTransaction(Transaction transaction)
        {
            User sender = _user_repository.GetUserById(transaction.sender);
            User receiver = _user_repository.GetUserById(transaction.receiver);

            _user_repository.SetUserBalance(sender, sender.balance - transaction.amount);
            _user_repository.SetUserBalance(receiver, receiver.balance + transaction.amount);

            transactionCollection.InsertOne(transaction);
        }
        public IEnumerable<Transaction> GetTransactions()
        {
            return transactionCollection.Find(new BsonDocument()).ToList();
        }
        public Transaction GetTransactionById(Guid id)
        {
            var filter = Builders<Transaction>.Filter.Eq(user => user.id, id);
            return transactionCollection.Find(filter).SingleOrDefault();
        }
        public void RevertTransaction(Guid id)
        {
            // data fetching
            var transaction = GetTransactionById(id);

            var userSender = _user_repository.GetUserById(transaction.sender);
            var userReceiver = _user_repository.GetUserById(transaction.receiver);

            // transaction operation
            _user_repository.SetUserBalance(userSender, (userSender.balance + transaction.amount));
            _user_repository.SetUserBalance(userReceiver, (userReceiver.balance - transaction.amount));

            Transaction newTransaction = new()
            {
                id = Guid.NewGuid(),
                sender = transaction.receiver,
                receiver = transaction.sender,
                createdAt = DateTimeOffset.UtcNow,
                amount = 0
            };

            transactionCollection.InsertOne(newTransaction);
        }
    }
}