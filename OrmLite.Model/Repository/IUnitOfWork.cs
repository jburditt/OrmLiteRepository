using System;
using System.Data;

namespace OrmLite.Model
{
    public interface IUnitOfWork : IDisposable
    {
        string ConnectionString { get; set; }
        IDbConnection Db { get; }
        IQuery Query { get; }
        IRepository Repository { get; }

        IUnitOfWork Open(string connectionString = null);
        void Close();
        void Dispose();
    }
}