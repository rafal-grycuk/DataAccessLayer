﻿using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DataAccessLayer.Core.Exceptions;
using DataAccessLayer.Core.Repositories.Concrete;
using DataAccessLayer.Core.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Core.UoW
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
            catch (Exception)
            {
                var validationErrors = _context.ChangeTracker
                                                .Entries<IValidatableObject>()
                                                .SelectMany(e => e.Entity.Validate(null))
                                                .Where(r => r != ValidationResult.Success);
                if (validationErrors.Any())
                {
                    var sb = new StringBuilder();
                    foreach (var entry in validationErrors)
                    {
                        foreach (var member in entry.MemberNames)
                        {
                            sb.AppendLine($"{entry.ErrorMessage}-{member}");
                        }
                    }
                    throw new Exception(sb.ToString());
                }
                else throw;
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
