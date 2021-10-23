using System;
using System.Collections.Generic;
using Bank.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Bank.Repositories
{
    class MongoDBUsersRepository : IUserRepository
    {
        private const string databaseName = "bank";
        private const string collectionName = "users";
        private readonly IMongoCollection<User> usersCollection;
        private readonly FilterDefinitionBuilder<User> filterBuilder = Builders<User>.Filter;
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
        public User GetUser(Guid id)
        {
            var filter = filterBuilder.Eq(user => user.id, id);
            return usersCollection.Find(filter).SingleOrDefault();
        }
        public double GetUserBalance(Guid id)
        {
            var filter = filterBuilder.Eq(user => user.id, id);
            var userById = usersCollection.Find(filter).SingleOrDefault();
            return userById.balance;
        }
        public string GetUserType(Guid id)
        {
            var filter = filterBuilder.Eq(user => user.id, id);
            var userById = usersCollection.Find(filter).SingleOrDefault();
            return userById.type;
        }
        public void SetUserBalance(User user, double newBalance)
        {
            var filter = filterBuilder.Eq(user => user.id, user.id);
            var userById = usersCollection.Find(filter).SingleOrDefault();
            var update = Builders<User>.Update.Set("balance", newBalance);
            usersCollection.UpdateOne(filter, update);
        }
    }

}