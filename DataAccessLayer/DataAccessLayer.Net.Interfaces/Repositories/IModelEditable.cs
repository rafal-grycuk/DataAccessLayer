using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataAccessLayer.Net.Interfaces.Repositories
{
    public interface IModelEditable<T>
    {
        bool Delete(T entity);

        bool DeleteRange(IEnumerable<T> entities);

        bool DeleteAll();

        T Add(T entity);

        T AddOrUpdate(Expression<Func<T, object>> predicate, T entity);

        void AddRange(IEnumerable<T> entities);

        T Update(T entity);

        IEnumerable<T> UpdateRange(IEnumerable<T> entities);
    }
}
