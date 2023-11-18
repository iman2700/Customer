using System.Linq.Expressions;
using Application.Persistence;
using Domain.Common;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

// Generic repository base class implementing IAsyncRepository for CRUD operations
public class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase
{
    
    public readonly CustomerContext _dbContext;

     
    public RepositoryBase(CustomerContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }
    // Retrieve an entity by its unique identifier asynchronously
    public virtual async Task<T> GetByIdAsync(int id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }
    // Retrieve all entities asynchronously
    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }

    // Retrieve entities based on a predicate asynchronously
    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbContext.Set<T>().Where(predicate).ToListAsync();
    }

    // Retrieve entities based on various parameters asynchronously
    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeString = null, bool disableTracking = true)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        // Optionally disable tracking for read-only operations
        if (disableTracking)
            query = query.AsNoTracking();

        // Include related entities if specified
        if (!string.IsNullOrWhiteSpace(includeString))
            query = query.Include(includeString);

        // Apply predicate if specified
        if (predicate != null)
            query = query.Where(predicate);

        // Apply ordering if specified
        if (orderBy != null)
            return await orderBy(query).ToListAsync();

        return await query.ToListAsync();
    }

    // Retrieve entities with additional include expressions asynchronously
    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<Expression<Func<T, object>>> includes = null, bool disableTracking = true)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        // Optionally disable tracking for read-only operations
        if (disableTracking)
            query = query.AsNoTracking();

        // Include related entities if specified
        if (includes != null)
            query = includes.Aggregate(query, (current, include) => current.Include(include));

        // Apply predicate if specified
        if (predicate != null)
            query = query.Where(predicate);

        // Apply ordering if specified
        if (orderBy != null)
            return await orderBy(query).ToListAsync();

        return await query.ToListAsync();
    }

    

    // Add a new entity asynchronously
    public async Task<T> AddAsync(T entity)
    {
        _dbContext.Set<T>().Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    // Update an existing entity asynchronously
    public async Task UpdateAsync(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    // Delete an entity asynchronously
    public async Task DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }
}
