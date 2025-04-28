using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PostIt.Application.Abstractions.Data;

namespace PostIt.Infrastructure.Data.Configuration.Repositories;

public class Repository<TEntity> : IRepository<TEntity> 
    where TEntity : class
{
    protected DbContext DbContext { get; }

    protected DbSet<TEntity> DbSet { get; }

    public Repository(DbContext context)
    {
        DbContext = context;
        DbSet = DbContext.Set<TEntity>();
    }
    
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entity);
        
        await DbContext.AddAsync(entity, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

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

    public async Task DeleteAsync(TEntity[] entities, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(entities);

        foreach (var entity in entities)
        {
            DbContext.Remove(entity);
        }

        await SaveChangesAsync(cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken,
        bool tracking = true)
    {
        var query = AsQueryable(tracking);
        
        return await query
            .SingleOrDefaultAsync(expression, cancellationToken);
    }
    
    public IQueryable<TEntity> AsQueryable(bool tracking = true)
    {
        return tracking 
            ? DbSet.AsQueryable<TEntity>()
            : DbSet.AsQueryable<TEntity>().AsNoTracking();
    }

    public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
    {
        ArgumentNullException.ThrowIfNull(expression);
        return DbSet.Where(expression);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken)
    {
        return await DbSet.AnyAsync(expression, cancellationToken);
    }


    public async Task<TEntity?> SingleOrDefaultAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken,
        bool tracking = true,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = AsQueryable(tracking);

        query = includes
            .Aggregate(query, (current, include) => current.Include(include));

        return await query.SingleOrDefaultAsync(expression, cancellationToken);
    }
    
    public async Task<List<TEntity>> ToListAsync(
        CancellationToken cancellationToken,
        Expression<Func<TEntity, bool>>? expression = null,
        bool tracking = true)
    {
        var query = AsQueryable(tracking);
        
        if (expression is not null)
        {
            query = query.Where(expression);
        }

        return await query.ToListAsync(cancellationToken);
    }
    
    private Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return DbContext.SaveChangesAsync(cancellationToken);
    }
}
