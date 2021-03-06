﻿using DataAccessLayer.Net.Interfaces.Repositories;

namespace DataAccessLayer.Net.Interfaces.UoW
{
    public interface IUnitOfWork
    {
        void Dispose();
        int Save();
        void Dispose(bool disposing);
        IRepository<T> Repository<T>() where T : class;
    }
}
