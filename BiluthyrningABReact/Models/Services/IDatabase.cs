using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningABReact.Models
{
    public interface IDatabase
    {
        Task<IMongoCollection<T>> GetCollectionFromDb<T>(string collection);

        Task<bool> UpdateDb<T>(FilterDefinition<T> filter, UpdateDefinition<T> update, IMongoCollection<T> collection);

        Task<bool> InsertIntoDb<T>(T row, IMongoCollection<T> collection);
    }
}

