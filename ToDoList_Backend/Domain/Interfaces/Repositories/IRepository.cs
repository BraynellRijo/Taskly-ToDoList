namespace Domain.Interfaces.Repositories
{
    public interface IRepository<T>
    {
        Task<T> GetValueAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task CreateAsync(T entity);
        Task UpdateAsync(int id, T newEntity);
        Task DeleteAsync(int id);
        
    }
}
