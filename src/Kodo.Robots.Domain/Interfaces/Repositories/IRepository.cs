using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Kodo.Robots.Domain.Interfaces.Repositories
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {

        TEntity Create(TEntity model);

        void Update(TEntity model);

        void Delete(TEntity model);

        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> where);

        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, object> includes);

        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where);

        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, object> includes);
    }
}
