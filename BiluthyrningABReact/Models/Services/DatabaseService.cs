using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace BiluthyrningABReact.Models
{
    internal static class DatabaseService
    {
        private const string connString = "mongodb://localhost:27017";
        private const string database = "BiluthyrningAB";

        internal static IMongoCollection<T> GetCollectionFromDb<T>(string collection)
        {
            var client = new MongoClient(connString);
            var dataBase = client.GetDatabase(database);
            return dataBase.GetCollection<T>(collection);
        }

        internal static async Task<bool> UpdateDb<T>(FilterDefinition<T> filter, UpdateDefinition<T> update, IMongoCollection<T> collection)
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

        internal static async Task<bool> InsertIntoDb<T>(T row, IMongoCollection<T> collection)
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
