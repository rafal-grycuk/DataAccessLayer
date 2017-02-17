using System;
using System.Collections;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Text;
using DataAccessLayer.Net.Exceptions;
using DataAccessLayer.Net.Repositories.Concrete;
using DataAccessLayer.Net.Repositories.Interfaces;

namespace DataAccessLayer.Net.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        private bool _disposed;
        private readonly Hashtable _repositories;

        public UnitOfWork(DbContext context)
        {
            _context = context;
            _repositories = new Hashtable();
        }
        
        public void Dispose()
        {
            Dispose();
            GC.SuppressFinalize(this);
        }

        public DbContext DbContext => _context;

        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                var innerEx = e.InnerException;

                while (innerEx?.InnerException != null)
                    innerEx = innerEx.InnerException;

                SqlException ex = innerEx as SqlException;
                if (ex != null)
                {
                    var sqlEx = ex;
                    switch (sqlEx.Errors[0].Number)
                    {
                        case 547:
                            throw new ForeignKeyViolationException(sqlEx.Errors[0].Message);
                        case 2601:
                            throw new PrimaryKeyViolationException(sqlEx.Errors[0].Message);
                        case 2627:
                            throw new UniqueKeyViolationException(sqlEx.Errors[0].Message);
                        default:
                            throw new Exception(sqlEx.Message.ToString());
                    }
                }
                else
                    throw;
            }
            catch (DbEntityValidationException e)
            {
                var sb = new StringBuilder();
                foreach (var entry in e.EntityValidationErrors)
                {
                    foreach (var error in entry.ValidationErrors)
                    {
                        sb.AppendLine($"{entry.Entry.Entity}-{error.PropertyName}-{error.ErrorMessage}"
                        );
                    }
                }
                throw new Exception(sb.ToString());
            }
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _context.Dispose();
            _disposed = true;
        }

        public IRepository<T> Repository<T>() where T : class
        {
           var type = typeof(T).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);
                _repositories.Add(type, repositoryInstance);
            }
            return _repositories[type] as IRepository<T>;
        }
    }
}
