using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using DataAccessLayer.Net.Interfaces.Repositories;

namespace DataAccessLayer.Net.EntityFramework.Repositories
{
    public class EntityFrameworkRepository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public EntityFrameworkRepository(DbContext context)
        {
            this._context = context;
            this._dbSet = context.Set<T>();
        }

        public T Add(T entity)
        {
            return _dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public int Count(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null ? _dbSet.Count() : _dbSet.Count(predicate);
        }

        public bool Delete(T entity)
        {
            try
            {
                if (_context.Entry(entity).State == EntityState.Detached)
                {
                    _dbSet.Attach(entity);
                }
                _dbSet.Remove(entity);
            }
            catch (Exception)
            {
                return false;
            }
            return true;

        }

        public bool DeleteRange(IEnumerable<T> entities)
        {
            try
            {
                foreach (var item in entities.Where(en => _context.Entry(en).State == EntityState.Detached))
                    _dbSet.Attach(item);
                _dbSet.RemoveRange(entities);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public T Update(T entity)
        {
            var dbEntityEntry = _context.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;
            return dbEntityEntry.Entity;
        }

        public IEnumerable<T> UpdateRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                if (_context.Entry(entity).State == EntityState.Detached)
                {
                    _dbSet.Attach(entity);
                }
                _context.Entry(entity).State = EntityState.Modified;
            }
            return entities;
        }

        private static IEnumerable<T> GetRangePrivate(Expression<Func<T, bool>> filterPredicate, Func<IQueryable<T>, IOrderedQueryable<T>> orderbyPredicate, Expression<Func<T, object>>[] tablePredicate, IQueryable<T> query, int? skip = null, int? take = null)
        {
            if (filterPredicate != null)
            {
                query = query.Where(filterPredicate);
            }

            if (tablePredicate != null)
            {
                foreach (var inc in tablePredicate)
                {
                    query = (IQueryable<T>)query.Include(inc);
                }
            }

            try
            {
                var invoked = orderbyPredicate != null ? orderbyPredicate.Invoke(query) : query;
                IQueryable<T> result = invoked;
                if (skip.HasValue)
                    result = result.Skip(skip.Value);
                if (take.HasValue)
                    result = result.Take(take.Value);
                return result.ToList();
            }
            catch (Exception)
            {
                throw;
            }


        }
        public T Get(int id, bool enableTracking = true, params Expression<Func<T, object>>[] tablePredicate)
        {
            IQueryable<T> query = enableTracking ? _dbSet : _dbSet.AsNoTracking();
            var param = Expression.Parameter(typeof(T));
            var lambda =
                Expression.Lambda<Func<T, bool>>(
                    Expression.Equal(Expression.Property(param, "Id"), Expression.Constant(id)), param);
            if (tablePredicate != null)
            {
                foreach (var item in tablePredicate)
                    query = (IQueryable<T>)query.Include(item);
            }
            return query.Where(lambda).SingleOrDefault();
        }

        public T Get(Expression<Func<T, bool>> filterPredicate, bool enableTracking = true, params Expression<Func<T, object>>[] tablePredicate)
        {
            IQueryable<T> query = enableTracking ? _dbSet : _dbSet.AsNoTracking(); ;
            var param = Expression.Parameter(typeof(T));
            var lambda = filterPredicate;
            if (tablePredicate != null)
            {
                foreach (var item in tablePredicate)
                    query = (IQueryable<T>)query.Include(item);
            }
            return query.Where(lambda).SingleOrDefault();
        }

        public IEnumerable<T> GetRange(Expression<Func<T, bool>> filterPredicate = null, bool enableTracking = true, Func<IQueryable<T>, IOrderedQueryable<T>> orderByPredicate = null, int? skip = null, int? take = null, params Expression<Func<T, object>>[] tablePredicate)
        {
            IQueryable<T> query = enableTracking ? _dbSet : _dbSet.AsNoTracking();
            return GetRangePrivate(filterPredicate, orderByPredicate, tablePredicate, query, skip, take);
        }

    }
}
