using System.Linq.Expressions;
using Core.Interfaces;

namespace BMO.API.Core.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(long id);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    void Update(T entity);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);

    public Task<IEnumerable<TResult>> GetBySpecificationAsync<TResult>(ISpecification<T, TResult> specification, int? offset = null, int? limit = null);

    Task SaveChangesAsync();
}