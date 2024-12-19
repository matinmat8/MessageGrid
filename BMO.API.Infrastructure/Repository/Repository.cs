using System.Linq.Expressions;
using BMO.API.Core.Interfaces;
using BMO.API.Infrastructure.Data;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
// using Core.Exceptions;
// using Infrastructure.Data;
// using Infrastructure.Data.EF;

namespace BMO.API.Infrastructure.Repository;

/// <summary>
/// A generic repository for performing basic CRUD operations on entities of type T.
/// It supports operations such as Get, Add, Update, Remove, and specialized queries like Find and Count.
/// </summary>
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext Context;
    protected readonly DbSet<T> DbSet;

    /// <summary>
    /// Constructor that initializes the repository with the given database context.
    /// </summary>
    /// <param name="context">The ApplicationDbContext instance used for database operations.</param>
    public Repository(ApplicationDbContext context)
    {
        Context = context;
        DbSet = Context.Set<T>();
    }

    /// <summary>
    /// Retrieves all entities of type T from the database.
    /// </summary>
    /// <returns>A list of all entities of type T.</returns>
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var ts = DbSet.ToList();
        return await DbSet.ToListAsync();
    }

    /// <summary>
    /// Retrieves an entity by its ID.
    /// Throws an exception if the entity is not found.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve.</param>
    /// <returns>The entity of type T with the specified ID.</returns>
    /// <exception cref="EntityNotFoundException">Thrown when the entity is not found.</exception>
    public async Task<T?> GetByIdAsync(long id)
    {
        var entity = await DbSet.FindAsync(id);
        if (entity == null)
        {
            throw new InvalidOperationException($"Entity of type {typeof(T).Name} with ID {id} was not found.");
        }

        return entity;
    }

    /// <summary>
    /// Finds entities that match the given predicate.
    /// </summary>
    /// <param name="predicate">The condition to filter entities.</param>
    /// <returns>A list of entities that match the predicate.</returns>
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.Where(predicate).ToListAsync();
    }

    /// <summary>
    /// Adds a new entity of type T to the database.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    public async Task AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
    }

    /// <summary>
    /// Adds a collection of new entities of type T to the database.
    /// </summary>
    /// <param name="entities">The entities to add.</param>
    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await DbSet.AddRangeAsync(entities);
    }

    /// <summary>
    /// Updates an existing entity of type T in the database.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    public void Update(T entity)
    {
        DbSet.Update(entity);
    }

    /// <summary>
    /// Removes an entity of type T from the database.
    /// </summary>
    /// <param name="entity">The entity to remove.</param>
    public void Remove(T entity)
    {
        DbSet.Remove(entity);
    }

    /// <summary>
    /// Removes an entity of type T from the database and saves changes.
    /// </summary>
    /// <param name="entity">The entity to remove.</param>
    public async Task RemoveAsync(T entity)
    {
        DbSet.Remove(entity);
        await SaveChangesAsync(); // Assuming removal is part of the same transaction
    }

    /// <summary>
    /// Removes a collection of entities of type T from the database.
    /// </summary>
    /// <param name="entities">The collection of entities to remove.</param>
    public void RemoveRange(IEnumerable<T> entities)
    {
        DbSet.RemoveRange(entities);
    }

    /// <summary>
    /// Counts the Recipient of entities that match the given predicate.
    /// </summary>
    /// <param name="predicate">The condition to count matching entities.</param>
    /// <returns>The count of entities that match the predicate.</returns>
    public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
    {
        return await DbSet.CountAsync(predicate);
    }

    /// <summary>
    /// Retrieves entities based on a specification that may include filtering, sorting, and includes,
    /// with optional pagination using offset and limit.
    /// </summary>
    /// <param name="specification">The specification that defines filtering, includes, sorting, etc.</param>
    /// <param name="offset">The Recipient of records to skip (default is 0).</param>
    /// <param name="limit">The maximum Recipient of records to return (default is no limit).</param>
    // /// <returns>A list of entities that match the specification.</returns>
    public async Task<IEnumerable<TResult>> GetBySpecificationAsync<TResult>(
        ISpecification<T, TResult> specification,
        int? offset = null,
        int? limit = null)
    {
        IQueryable<T> query = DbSet;

        // Apply criteria (filtering) if available in the specification
        if (specification.Criteria != null)
        {
            query = query.Where(specification.Criteria);
        }

        // Apply includes (navigation properties) if available in the specification
        foreach (var include in specification.Includes)
        {
            query = query.Include(include);
        }

        // Apply sorting if available in the specification
        if (specification.OrderBy != null)
        {
            query = query.OrderBy(specification.OrderBy);
        }

        // Apply pagination: offset and limit
        if (offset is > 0)
        {
            query = query.Skip(offset.Value);
        }

        if (limit is > 0)
        {
            query = query.Take(limit.Value);
        }

        // If a Selector is provided, use it to project the results to the desired TResult type
        if (specification.Selector != null)
        {
            return await query.Select(specification.Selector).ToListAsync();
        }

        // Otherwise, return the full entities as T
        return await query.ToListAsync() as IEnumerable<TResult> ?? new List<TResult>();
    }


    /// <summary>
    /// Saves all changes made to the database context.
    /// </summary>
    public async Task SaveChangesAsync()
    {
        await Context.SaveChangesAsync();
    }

}