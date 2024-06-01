using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nova_mas_blog_api.Services
{
    public class BaseService<T> where T : class
    {
        protected readonly IMongoCollection<T> _collection;

        public BaseService(IMongoCollection<T> collection)
        {
            _collection = collection;
        }

        public async Task<IEnumerable<T>> GetAll(int page, int pageSize)
        {
            return await _collection.Find(_ => true)
                                    .Skip((page - 1) * pageSize)
                                    .Limit(pageSize)
                                    .ToListAsync();
        }

        public async Task<T> GetById(string id)
        {
            return await _collection.Find(Builders<T>.Filter.Eq("Id", id)).FirstOrDefaultAsync();
        }

        public async Task<T> Create(T entity)
        {
            await _collection.InsertOneAsync(entity);
            return entity;
        }

        public async Task<T> Update(string id, T entity)
        {
            var result = await _collection.ReplaceOneAsync(Builders<T>.Filter.Eq("Id", id), entity);
            if (result.ModifiedCount == 0)
            {
                throw new InvalidOperationException("Failed to update the entity.");
            }
            return entity;
        }

        public async Task<bool> Delete(string id)
        {
            var result = await _collection.DeleteOneAsync(Builders<T>.Filter.Eq("Id", id));
            return result.DeletedCount > 0;
        }
    }
}
