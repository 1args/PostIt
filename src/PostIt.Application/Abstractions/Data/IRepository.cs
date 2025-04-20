using System.Linq.Expressions;

namespace PostIt.Application.Abstractions.Data;

public interface IRepository<TEntity> where TEntity : class
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    
    Task DeleteAsync(TEntity[] entities, CancellationToken cancellationToken);
    
    IQueryable<TEntity> AsQueryable(bool tracking = true);
    
    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);

    Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken,
        bool tracking = true);

    Task<List<TEntity>> ToListAsync(
        CancellationToken cancellationToken,
        Expression<Func<TEntity, bool>>? expression = null,
        bool tracking = true);
}