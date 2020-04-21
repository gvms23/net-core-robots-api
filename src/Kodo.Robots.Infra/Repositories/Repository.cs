using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Kodo.Robots.Domain.Entities;
using Kodo.Robots.Domain.Interfaces;
using Kodo.Robots.Domain.Interfaces.Repositories;
using Kodo.Robots.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Kodo.Robots.Infra.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly RobotsContext _context;

        private DbSet<TEntity> DbSet => _context.Set<TEntity>();

        public Repository(RobotsContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> Query()
        {
            return DbSet;
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> where)
        {
            return await DbSet.FirstOrDefaultAsync(@where);
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, object> includes)
        {
            IQueryable<TEntity> _query = DbSet;

            if (includes != null)
                _query = includes(_query) as IQueryable<TEntity>;

            return await (_query ?? throw new InvalidOperationException()).FirstOrDefaultAsync(predicate);
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where)
        {
            return DbSet.Where(@where);
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, object> includes)
        {
            IQueryable<TEntity> _query = DbSet;

            if (includes != null)
                _query = includes(_query) as IQueryable<TEntity>;

            return (_query ?? throw new InvalidOperationException()).Where(predicate).AsQueryable();
        }

        public TEntity Create(TEntity model)
        {
            DbSet.Add(model);
            return model;
        }

        public void Update(TEntity model)
        {
            var entry = _context.Entry(model);

            DbSet.Attach(model);

            entry.State = EntityState.Modified;
        }

        public void Delete(TEntity model)
        {
            if (model is EntityBase _entityBase)
            {
                _entityBase.IsDeleted = true;
                var _entry = _context.Entry(model);

                DbSet.Attach(model);

                _entry.State = EntityState.Modified;
            }
            else
            {
                var _entry = _context.Entry(model);
                DbSet.Attach(model);
                _entry.State = EntityState.Deleted;
            }
        }

        public void Dispose()
         {
             _context?.Dispose();
             GC.SuppressFinalize(this);
         }
    }
}