using System;
using ServiceStack.OrmLite;
using System.Data;
using OrmLite.Model;

namespace OrmLite.Repository
{
    /// <summary>
    /// TODO
    /// - Move Poco T4 to new project OrmLite.Model
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        public static string ConnectionString { private get; set; }
        string IUnitOfWork.ConnectionString
        {
            get { return ConnectionString; }
            set { ConnectionString = value; }
        }

        public IDbConnection Db { get; private set; }
        public IQuery Query { get; private set; }
        public IRepository Repository { get; private set; }
        public ITableStorageRepository TableStorageRepository { get; private set; }

        static UnitOfWork()
        {
            OrmLiteConfig.DialectProvider = SqlServerDialect.Provider;
        }

        public UnitOfWork(string connectionString = null)
        {
            if (!string.IsNullOrEmpty(connectionString))
                ConnectionString = connectionString;
        }

        public IUnitOfWork Open(string connectionString = null)
        {
            if (string.IsNullOrEmpty(connectionString))
                connectionString = ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("UnitOfWork.Open - Connection String is NULL.");

            Db = connectionString.OpenDbConnection();
            
            Query = new Query(Db);
            Repository = new OrmLiteRepository(Db);
            TableStorageRepository = new OrmLiteTableStorageRepository(Db);

            return this;
        }

        public void Close()
        {
            Db.Close();
        }

        public void Dispose()
        {
            Db.Dispose();
        }
    }
}