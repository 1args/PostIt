using System.Linq.Expressions;

namespace PostIt.Infrastructure.Configuration.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);
    
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    
    Task DeleteAsync(TEntity[] entities, CancellationToken cancellationToken);
    
    IQueryable<TEntity> AsQueryable();
    
    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);
}