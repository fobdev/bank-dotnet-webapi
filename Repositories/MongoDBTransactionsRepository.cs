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
        public MongoDBTransactionsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            transactionCollection = database.GetCollection<Transaction>(collectionName);
        }
        public void CreateTransaction(User sender, User receiver, double amount)
        {
            throw new NotImplementedException();
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