using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace BiluthyrningABReact.Models
{
    public class DatabaseRepository : IDatabase
    {
        public async Task<IMongoCollection<T>> GetCollectionFromDb<T>(string collection)
        {
            return null;
        }

        public async Task<long> UpdateDb<T>(FilterDefinition<T> filter, UpdateDefinition<T> update, IMongoCollection<T> collection)
        {
            try
            {
                var result = await collection.UpdateOneAsync(filter, update);
                return result.ModifiedCount;
            }
            catch (Exception e)
            {
                var error = e;
                return -1;
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
