using System.Collections.Generic;

namespace DataAccessLayer.Core.Interfaces.Repositories
{
    public interface IModelEditable<T>
    {
        bool Delete(T entity);

        bool DeleteRange(IEnumerable<T> entities);

        T Add(T entity);

        void AddRange(IEnumerable<T> entities);

        T Update(T entity);

        IEnumerable<T> UpdateRange(IEnumerable<T> entities);
    }
}
