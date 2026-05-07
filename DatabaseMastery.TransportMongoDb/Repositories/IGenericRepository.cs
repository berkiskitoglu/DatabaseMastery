using DatabaseMastery.TransportMongoDb.Core.Interfaces;
using MongoDB.Driver;

namespace DatabaseMastery.TransportMongoDb.Repositories
{
    public interface IGenericRepository<T> where T : IEntity
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(string id);
        Task CreateAsync(T entity);
        Task DeleteAsync(string id);
        Task UpdateAsync(T entity);

        Task<T> GetByFilterAsync(FilterDefinition<T> filter);
        Task<long> CountDocumentsAsync(FilterDefinition<T> filter);
        Task<List<TField>> GetDistinctAsync<TField>(string field, FilterDefinition<T> filter);
        Task UpdateByFilterAsync(FilterDefinition<T> filter, UpdateDefinition<T> update);

    }
}
