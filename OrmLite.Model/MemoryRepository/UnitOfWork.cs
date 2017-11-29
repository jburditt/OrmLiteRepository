using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;

namespace OrmLite.Model
{
    public class UnitOfWork : IUnitOfWork
    {
        private ConcurrentDictionary<Type, List<object>> _db = new ConcurrentDictionary<Type, List<object>>();

        public string ConnectionString { get; set; }
        public IQuery Query { get { throw new NotImplementedException(); } }
        public IRepository Repository { get; private set; }
        public IDbConnection Db { get { throw new NotImplementedException(); } }

        public UnitOfWork(string connectionString = null)
        {
            if (!string.IsNullOrEmpty(connectionString))
                ConnectionString = connectionString;
        }

        public IUnitOfWork Open(string connectionString = null)
        {
            // TODO Connection string needs to be an index for the DB so that each connection string has a different DB
            Repository = new MemoryRepository(_db);

            return this;
        }

        public void Close()
        {
            if (_db != null)
                _db.Clear();
        }

        public void Dispose()
        {
            if (_db != null)
                _db = null;
        }
    }
}