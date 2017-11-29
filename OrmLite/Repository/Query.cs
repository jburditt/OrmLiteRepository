using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack.OrmLite;
using OrmLite.Model;

namespace OrmLite.Repository
{
    public class Query : IQuery
    {
        protected IDbConnection db;

        public Query(IDbConnection db)
        {
            this.db = db;
        }

        #region Sync

        public virtual void Execute(string sql)
        {
            db.ExecuteSql(sql);
        }

        public virtual object Scalar<T>(string sql)
        {
            return db.SqlScalar<object>(sql);
        }

        public virtual T Get<T>(string sql)
        {
            return db.SqlList<T>(sql).FirstOrDefault();
        }

        public virtual T Get<T>(string sql, params IDbDataParameter[] sqlParams)
        {
            return db.SqlList<T>(sql, sqlParams).FirstOrDefault();
        }

        public virtual IEnumerable<T> Find<T>(string sql)
        {
            return db.SqlList<T>(sql);
        }

        public virtual IEnumerable<T> Find<T>(T n, string sql)
        {
            return db.SqlList<T>(sql);
        }

        public virtual IEnumerable<T> Find<T>(string sql, params IDbDataParameter[] sqlParams)
        {
            return db.SqlList<T>(sql, sqlParams);
        }

        #endregion Sync

        #region Async

        public virtual async Task<int> ExecuteAsync(string sql)
        {
            return await db.ExecuteSqlAsync(sql);
        }

        public virtual async Task<object> ScalarAsync<T>(string sql)
        {
            return await db.SqlScalarAsync<object>(sql);
        }

        public virtual async Task<T> GetAsync<T>(string sql)
        {
            //return await db.SqlListAsync<T>(sql).FirstOrDefault();
            throw new NotImplementedException();
        }

        public virtual async Task<T> GetAsync<T>(string sql, params IDbDataParameter[] sqlParams)
        {
            //return db.SqlList<T>(sql, sqlParams).FirstOrDefault();
            throw new NotImplementedException();
        }

        public virtual async Task<IEnumerable<T>> FindAsync<T>(string sql)
        {
            return await db.SqlListAsync<T>(sql);
        }

        public virtual async Task<IEnumerable<T>> FindAsync<T>(string sql, params IDbDataParameter[] sqlParams)
        {
            return await db.SqlListAsync<T>(sql, sqlParams);
        }

        #endregion Async
    }
}
