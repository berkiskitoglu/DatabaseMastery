using DatabaseMastery.TransportMongoDb.Core.Interfaces;

namespace DatabaseMastery.TransportMongoDb.Repositories
{
    public interface IGenericRepository<T> where T : IEntity
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(string id);
        Task CreateAsync(T entity);
        Task DeleteAsync(string id);
        Task UpdateAsync(T entity);
    }
}
