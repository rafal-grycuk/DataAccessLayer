using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Core.Interfaces.Repositories
{
    public interface IModelEditable<T>
    {
        bool Delete(T entity);

        bool DeleteRange(IEnumerable<T> entities);

        bool DeleteAll();

        T Add(T entity);

        void AddOrUpdate(Expression<Func<T, bool>> predicate, T entity);

        void AddRange(IEnumerable<T> entities);

        T Update(T entity);

        IQueryable<T> UpdateRange(IEnumerable<T> entities);
    }
}
