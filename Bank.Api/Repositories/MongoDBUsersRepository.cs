using System;
using System.Collections.Generic;
using Bank.Api.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Bank.Api.Repositories
{
    class MongoDBUsersRepository : IUserRepository
    {
        private const string databaseName = "bank";
        private const string collectionName = "users";
        private readonly IMongoCollection<User> usersCollection;
        public MongoDBUsersRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            usersCollection = database.GetCollection<User>(collectionName);
        }
        public void CreateUser(User user)
        {
            usersCollection.InsertOne(user);
        }
        public IEnumerable<User> GetUsers()
        {
            return usersCollection.Find(new BsonDocument()).ToList();
        }
        public User GetUserById(Guid id)
        {
            var filter = Builders<User>.Filter.Eq(user => user.id, id);
            return usersCollection.Find(filter).SingleOrDefault();
        }
        public double GetUserBalance(Guid id)
        {
            var filter = Builders<User>.Filter.Eq(user => user.id, id);
            var userById = usersCollection.Find(filter).SingleOrDefault();
            return userById.balance;
        }
        public bool GetUserType(Guid id)
        {
            var filter = Builders<User>.Filter.Eq(user => user.id, id);
            var userById = usersCollection.Find(filter).SingleOrDefault();
            return userById.staff;
        }
        public void SetUserBalance(Guid user, double transactionAmount, bool positive)
        {
            var filter = Builders<User>.Filter.Eq(user => user.id, user);
            var userById = usersCollection.Find(filter).SingleOrDefault();

            UpdateDefinition<User> newBalance;
            if (positive) newBalance = Builders<User>.Update.Set("balance", userById.balance + transactionAmount);
            else newBalance = Builders<User>.Update.Set("balance", userById.balance - transactionAmount);

            usersCollection.UpdateOne(filter, newBalance);
        }

        public bool ExistsEmail(string email)
        {
            var filter = Builders<User>.Filter.Eq(user => user.email, email);
            var userById = usersCollection.Find(filter).SingleOrDefault();

            if (userById is not null) return true;

            return false;
        }

        public bool ExistsCPF(string cpf)
        {
            var filter = Builders<User>.Filter.Eq(user => user.cpf, cpf);
            var userById = usersCollection.Find(filter).SingleOrDefault();

            if (userById is not null) return true;

            return false;
        }
    }

}