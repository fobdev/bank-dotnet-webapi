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
        public IEnumerable<Transaction> GetTransactions()
        {
            return transactionCollection.Find(new BsonDocument()).ToList();
        }
        public Transaction GetTransactionById(Guid id)
        {
            var filter = Builders<Transaction>.Filter.Eq(user => user.id, id);
            return transactionCollection.Find(filter).SingleOrDefault();
        }
        public void CreateTransaction(Transaction transaction)
        {
            _user_repository.SetUserBalance(transaction.receiver, transaction.amount, true);
            _user_repository.SetUserBalance(transaction.sender, transaction.amount, false);

            transactionCollection.InsertOne(transaction);
        }
        public void RevertTransaction(Guid id)
        {
            Transaction oldTransaction = GetTransactionById(id);

            CreateTransaction(new()
            {
                id = Guid.NewGuid(),
                sender = oldTransaction.receiver,
                receiver = oldTransaction.sender,
                amount = oldTransaction.amount,
                createdAt = DateTimeOffset.UtcNow
            });
        }
        public bool CheckUserIsStaff(Guid id)
        {
            return _user_repository.GetUserById(id).premium;
        }
        public bool CheckTransactionSenderBalance(TransactionCreateDto transaction)
        {
            return _user_repository.GetUserById(transaction.sender).balance < transaction.amount;
        }
    }
}