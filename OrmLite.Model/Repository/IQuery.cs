using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace OrmLite.Model
{
    public interface IQuery
    {
        void Execute(string sql);
        object Scalar<T>(string sql);
        T Get<T>(string sql);
        T Get<T>(string sql, params IDbDataParameter[] sqlParams);
        IEnumerable<T> Find<T>(string sql);
        IEnumerable<T> Find<T>(string sql, params IDbDataParameter[] sqlParams);

        Task<int> ExecuteAsync(string sql);
        Task<object> ScalarAsync<T>(string sql);
        Task<T> GetAsync<T>(string sql);
        Task<T> GetAsync<T>(string sql, params IDbDataParameter[] sqlParams);
        Task<IEnumerable<T>> FindAsync<T>(string sql);
        Task<IEnumerable<T>> FindAsync<T>(string sql, params IDbDataParameter[] sqlParams);
    }
}