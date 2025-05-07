using System.Linq.Expressions;

namespace PostIt.Application.Abstractions.Data;

public interface IRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> AsQueryable();
    
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    
    Task DeleteAsync(TEntity[] entities, CancellationToken cancellationToken);
    
    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);
}