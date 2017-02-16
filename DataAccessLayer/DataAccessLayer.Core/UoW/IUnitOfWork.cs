using DataAccessLayer.Core.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Core.UoW
{
    public interface IUnitOfWork
    {
        DbContext DbContext { get; }
        void Dispose();
        void Save();
        void Dispose(bool disposing);
        IRepository<T> Repository<T>() where T : class;
    }
}
