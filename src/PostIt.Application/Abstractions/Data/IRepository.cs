using System.Linq.Expressions;

namespace PostIt.Application.Abstractions.Data;

public interface IRepository<TEntity> where TEntity : class
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    
    Task DeleteAsync(TEntity[] entities, CancellationToken cancellationToken);

    Task<TEntity> GetByIdAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken);
    
    IQueryable<TEntity> AsQueryable(bool tracking = true);
    
    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);

   Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken);
   
    Task<TEntity?> SingleOrDefaultAsync(
        Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken,
        bool tracking = true,
        params Expression<Func<TEntity, object>>[] includes);

    Task<List<TEntity>> ToListAsync(
        CancellationToken cancellationToken,
        Expression<Func<TEntity, bool>>? expression = null,
        bool tracking = true);

}