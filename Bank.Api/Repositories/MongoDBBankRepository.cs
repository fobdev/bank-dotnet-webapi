using System;
using Bank.Api.Models;
using MongoDB.Driver;

namespace Bank.Api.Repositories
{
    class MongoDBBankRepository : IBankRepository
    {
        private const string databaseName = "bank";
        private const string collectionName = "physicalbank";
        private readonly IMongoCollection<PhysicalBank> bankCollection;
        public MongoDBBankRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            bankCollection = database.GetCollection<PhysicalBank>(collectionName);
        }
        public void CreateBank(PhysicalBank bank)
        {
            bankCollection.InsertOne(bank);
        }

        public PhysicalBank GetBank(Guid id)
        {
            var filter = Builders<PhysicalBank>.Filter.Eq(bank => bank.id, id);
            return bankCollection.Find(filter).SingleOrDefault();
        }

        public void MakeLoan(LoanCreateDto loanCreateDto)
        {
            throw new NotImplementedException();
        }
    }
}