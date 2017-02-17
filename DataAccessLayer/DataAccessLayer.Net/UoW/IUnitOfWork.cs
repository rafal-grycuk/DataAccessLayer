using System.Data.Entity;
using DataAccessLayer.Net.Repositories.Interfaces;

namespace DataAccessLayer.Net.UoW
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
