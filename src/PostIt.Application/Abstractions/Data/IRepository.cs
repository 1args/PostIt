using System.Linq.Expressions;

namespace PostIt.Application.Abstractions.Data;

public interface IRepository<TEntity> where TEntity : class
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    
    Task DeleteAsync(TEntity[] entities, CancellationToken cancellationToken);
    
    IQueryable<TEntity> AsQueryable(bool tracking = true);
    
    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);
}