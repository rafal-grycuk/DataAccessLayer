using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccessLayer.Net.Interfaces.Repositories
{
    public interface IModelReadable<T>
    {
        int Count(Expression<Func<T, bool>> predicate = null);
        T Get(int id, bool enableTracking = true, params Expression<Func<T, object>>[] tablePredicate);

        T Get(Expression<Func<T, bool>> filterPredicate, bool enableTracking = true, params Expression<Func<T, object>>[] tablePredicate);

        IEnumerable<T> GetRange(Expression<Func<T, bool>> filterPredicate = null, bool enableTracking = true, Func<IQueryable<T>, IOrderedQueryable<T>> orderByPredicate = null, int? skip = null, int? take = null, params Expression<Func<T, object>>[] tablePredicate);
    }
}
