using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace BiluthyrningABReact.Models
{
    internal class DatabaseService : IDatabase
    {
        IConfiguration configuration;
        public DatabaseService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<IMongoCollection<T>> GetCollectionFromDb<T>(string collection)
        {
            string connString = configuration["connString"];/*"mongodb://localhost:27017";*/
            string database = configuration["dataBase"];/* "BiluthyrningAB";*/
            var client = new MongoClient(connString);
            var dataBase = client.GetDatabase(database);
            return dataBase.GetCollection<T>(collection);
        }

        public async Task<bool> UpdateDb<T>(FilterDefinition<T> filter, UpdateDefinition<T> update, IMongoCollection<T> collection)
        {
            try
            {
                var result = await collection.UpdateOneAsync(filter, update);
                return result.IsModifiedCountAvailable;
            }
            catch (Exception e)
            {
                var error = e;
                return false;
            }
        }

        public async Task<bool> InsertIntoDb<T>(T row, IMongoCollection<T> collection)
        {
            try
            {
                await collection.InsertOneAsync(row);
                return true;
            }
            catch (Exception e)
            {
                var error = e;
                return false;
            }
        }

    }
}
