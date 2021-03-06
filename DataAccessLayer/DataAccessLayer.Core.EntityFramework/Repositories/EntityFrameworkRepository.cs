﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DataAccessLayer.Core.EntityFramework.Utilities;
using DataAccessLayer.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Core.EntityFramework.Repositories
{
    public class EntityFrameworkRepository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public EntityFrameworkRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public T Add(T entity)
        {
            return _dbSet.Add(entity).Entity;
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public void AddOrUpdate(Expression<Func<T, bool>> predicate, T entity)
        {
            var filteredEntity = _context.Set<T>().FirstOrDefaultAsync(predicate).Result;
            if (filteredEntity == null)
                _context.Entry(entity).State = EntityState.Added;
            else
            {
                var primaryKeyNames = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(x => x.Name).ToList();
                _context.Entry(filteredEntity).State = EntityState.Detached;
                primaryKeyNames.ForEach(pk =>
                {
                    var primaryKeyValue = filteredEntity.GetType().GetProperty(pk).GetValue(filteredEntity, null);

                    entity.GetType().GetProperty(pk).SetValue(entity, primaryKeyValue);
                });
                _context.Entry(entity).State = EntityState.Modified;
            }
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
        public bool DeleteAll()
        {
            _context.Set<T>().RemoveRange(_context.Set<T>());
            return true;
        }
        public bool DeleteRange(IEnumerable<T> entities)
        {
            try
            {
                var enumerable = entities.ToList();
                foreach (var item in enumerable.Where(en => _context.Entry(en).State == EntityState.Detached))
                    _dbSet.Attach(item);
                _dbSet.RemoveRange(enumerable);
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
            var enumerable = entities.ToList();
            foreach (var entity in enumerable)
            {
                if (_context.Entry(entity).State == EntityState.Detached)
                {
                    _dbSet.Attach(entity);
                }
                _context.Entry(entity).State = EntityState.Modified;
            }
            return enumerable.AsQueryable();
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
                {
                    dynamic body = item.Body;
                    string includeString = GetPropertyMap(body);
                    query = query.Include(includeString);
                }
            }
            return query.Where(lambda).SingleOrDefault();
        }

        public T Get(Expression<Func<T, bool>> filterPredicate, bool enableTracking = true, params Expression<Func<T, object>>[] tablePredicate)
        {
            IQueryable<T> query = enableTracking ? _dbSet : _dbSet.AsNoTracking(); 
            var lambda = filterPredicate;
            if (tablePredicate != null)
            {
                foreach (var item in tablePredicate)
                {
                    dynamic body = item.Body;
                    string includeString = GetPropertyMap(body);
                    query = query.Include(includeString);
                }
            }
            return query.Where(lambda).SingleOrDefault();
        }

        private static string GetPropertyMap(dynamic body)
        {
            string includeString = "";
            if (Helpers.IsPropertyExist(body, "Arguments"))
            {
                dynamic arguments = body.Arguments;
                foreach (dynamic arg in arguments)
                {
                    if (Helpers.IsPropertyExist(arg, "Member"))
                    {
                        includeString += arg.Member.Name;
                    }
                    else if (Helpers.IsPropertyExist(arg, "Body") && Helpers.IsPropertyExist(arg.Body, "Member"))
                    {
                        includeString += "." + arg.Body.Member.Name;
                    }
                    else if (Helpers.IsPropertyExist(arg, "Body"))
                    {
                        includeString += "." + GetPropertyMap(arg.Body);
                    }
                }
            }
            else if (Helpers.IsPropertyExist(body, "Member"))
            {
                includeString += body.ToString().Substring(body.ToString().IndexOf(".") + 1);
            }
            return includeString;
        }

        public IEnumerable<T> GetRange(Expression<Func<T, bool>> filterPredicate = null, bool enableTracking = true, Func<IQueryable<T>, IOrderedQueryable<T>> orderByPredicate = null, int? skip = null, int? take = null, params Expression<Func<T, object>>[] tablePredicate)
        {
            IQueryable<T> query = enableTracking ? _dbSet : _dbSet.AsNoTracking();
            if (filterPredicate != null)
                query = query.Where(filterPredicate);

            if (tablePredicate != null)
            {
                foreach (var inc in tablePredicate)
                {
                    dynamic body = inc.Body;
                    string includeString = GetPropertyMap(body);
                    includeString = includeString.StartsWith(".") ? includeString.Remove(0, 1) : includeString;
                    includeString = includeString.EndsWith(".") ? includeString.Remove(includeString.Length - 1, 1) : includeString;

                    if (string.IsNullOrWhiteSpace(includeString) == false)
                        query = query.Include(includeString);
                }
            }
            var invoked = orderByPredicate != null ? orderByPredicate.Invoke(query) : query;
            IQueryable<T> result = invoked;
            if (skip.HasValue)
                result = result.Skip(skip.Value);
            if (take.HasValue)
                result = result.Take(take.Value);
            return result.AsQueryable();
        }
    }
}
