using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OrmLite.Model
{
    public interface IRepository
    {
        IEnumerable<T> All<T>();
        int Count<T>();

        T Get<T>(int id) where T : IHasId<int>;
        T Get<T>(Expression<Func<T, bool>> predicates);
        Tuple<T, TI> GetInclude<T, TI>(Expression<Func<T, TI, bool>> joinOn);
        Tuple<T, TI> GetInclude<T, TI>(Expression<Func<T, TI, bool>> joinOn, Expression<Func<T, TI, bool>> predicates);
        Tuple<T, TI> GetInclude<T, TI>(Expression<Func<T, TI, bool>> joinOn, Expression<Func<T, TI, bool>> predicates, Expression<Func<TI, object>> orderBy);

        IEnumerable<T> Find<T>(Expression<Func<T, bool>> predicates);
        IEnumerable<Tuple<T, TI>> FindInclude<T, TI>(Expression<Func<T, TI, bool>> joinOn, Expression<Func<T, TI, bool>> predicates);
        IEnumerable<Tuple<T, TI>> FindInclude<T, TI>(Expression<Func<T, TI, bool>> joinOn, Expression<Func<T, TI, bool>> predicates, Expression<Func<TI, object>> orderBy);

        void Insert_NoReturnId<T>(T obj);
        int Insert<T>(T obj) where T : IHasId<int>;
        //TId Insert<T, TId>(T obj, TId id) where T : IHasId<TId>;
        int InsertRange<T>(IEnumerable<T> objs);
        int Delete<T>(int id) where T : IHasId<int>;
        void Delete<T>(Expression<Func<T, bool>> predicates);   // TODO change all of these voids to INT (Id of item deleted / updated or count of items deleted / counted)
        void DeleteAll<T>();
        void Update<T>(T obj);
        void Update<T>(int id, params Func<T, object>[] properties);
        void Update<T>(long id, params Func<T, object>[] properties);
        int Upsert<T>(T obj) where T : IHasId<int>;
        TId Upsert<T, TId>(TId id, T obj) where T : IHasId<TId>;

        Task<IEnumerable<T>> AllAsync<T>();
        Task<int> CountAsync<T>();
        Task<T> GetAsync<T>(int id) where T : IHasId<int>;
        Task<T> GetAsync<T>(Expression<Func<T, bool>> predicates);
        Task<Tuple<T, TI>> GetIncludeAsync<T, TI>(Expression<Func<T, TI, bool>> joinOn);
        Task<Tuple<T, TI>> GetIncludeAsync<T, TI>(Expression<Func<T, TI, bool>> joinOn, Expression<Func<T, bool>> predicates);
        Task<Tuple<T, TI>> GetIncludeAsync<T, TI>(Expression<Func<T, TI, bool>> joinOn, Expression<Func<T, bool>> predicates, Expression<Func<TI, object>> orderBy);
        Task<IEnumerable<T>> FindAsync<T>(Expression<Func<T, bool>> predicates);
        Task<IEnumerable<Tuple<T, TI>>> FindIncludeAsync<T, TI>(Expression<Func<T, TI, bool>> joinOn, Expression<Func<T, bool>> predicates);
        Task<bool> Insert_NoReturnIdAsync<T>(T obj);
        Task<int> InsertAsync<T>(T obj) where T : IHasId<int>;
        Task<int> DeleteAsync<T>(int id);
        Task<int> DeleteAsync<T>(Expression<Func<T, bool>> predicates);
        Task<int> UpdateAsync<T>(T obj);
        Task<int> UpdateAsync<T>(int Id, params Func<T, object>[] properties);
        Task<bool> UpsertAsync<T>(T obj) where T : IHasId<int>;
    }
}