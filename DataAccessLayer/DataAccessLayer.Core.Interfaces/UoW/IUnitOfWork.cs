﻿using DataAccessLayer.Core.Interfaces.Repositories;

namespace DataAccessLayer.Core.Interfaces.UoW
{
    public interface IUnitOfWork
    {
        void Dispose();
        int Save();
        void Dispose(bool disposing);
        IRepository<T> Repository<T>() where T : class;
    }
}
