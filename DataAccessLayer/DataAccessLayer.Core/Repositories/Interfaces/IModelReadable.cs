using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccessLayer.Core.Repositories.Interfaces
{
    public interface IModelReadable<T>
    {
        T Get(int id, bool enableTracking = true, params Expression<Func<T, object>>[] tablePredicate);

        T Get(Expression<Func<T, bool>> filterPredicate, bool enableTracking = true, params Expression<Func<T, object>>[] tablePredicate);

        IEnumerable<T> GetRange(Expression<Func<T, bool>> filterPredicate = null, bool enableTracking = true, Func<IQueryable<T>, IOrderedQueryable<T>> orderByPredicate = null, params Expression<Func<T, object>>[] tablePredicate);
    }
}
