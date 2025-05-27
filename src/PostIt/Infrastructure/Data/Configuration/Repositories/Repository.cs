using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PostIt.Application.Abstractions.Data;

namespace PostIt.Infrastructure.Data.Configuration.Repositories;

/// <inheritdoc/>
public class Repository<TEntity> : IRepository<TEntity> 
    where TEntity : class
{
    /// <summary>
    /// Database context.
    /// </summary>
    private DbContext DbContext { get; }

    /// <summary>
    /// Storage of entities./>
    /// </summary>
    private DbSet<TEntity> DbSet { get; }

    public Repository(DbContext dbContext)
    {
        DbContext = dbContext;
        DbSet = DbContext.Set<TEntity>();
    }
    
    /// <inheritdoc/>
    public IQueryable<TEntity> AsQueryable()
    {
        return DbSet.AsQueryable();
    }
    
    /// <inheritdoc/>
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        await DbContext.AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        var state = DbContext.Entry(entity).State;

        if (state == EntityState.Detached)
        {
            DbContext.Attach(entity);
        }

        DbContext.Update(entity);
        return SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task UpdateRangeAsync(TEntity[] entities, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entities);
        
        foreach (var entity in entities)
        {
            if (DbContext.Entry(entity).State == EntityState.Detached)
            {
                DbContext.Attach(entity);
            }
            DbContext.Update(entity);
        }
        return SaveChangesAsync(cancellationToken);
    }
    
    /// <inheritdoc/>
    public async Task DeleteAsync(TEntity[] entities, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entities);

        foreach (var entity in entities)
        {
            DbContext.Remove(entity);
        }

        await SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        return DbSet.Where(expression);
    }

    private Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return DbContext.SaveChangesAsync(cancellationToken);
    }
}
