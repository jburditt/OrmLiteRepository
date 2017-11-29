using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OrmLite.Model
{
    public interface ITableStorageRepository
    {
        IEnumerable<T> All<T>();
        int Count<T>();
        T Get<T>(long key) where T : IHasId<long>;
        T Get<T>(Expression<Func<T, bool>> exp);
        T GetLatest<T>(long key) where T : IHasId<long>;
        long Insert<T>(T obj) where T : IHasId<long>;
        //void Insert<T, TId>(T obj, TId id) where T : IHasId<TId>;
        int InsertRange<T>(IEnumerable<T> objs) where T : IHasId<long>;
        void Delete<T>(long key) where T : IHasId<long>;
        int DeleteAll<T>();
        void Update<T>(T obj) where T : IHasId<long>;
        void Update<T>(long Id, params Func<T, object>[] properties);
        long Upsert<T>(T obj) where T : IHasId<long>;
        //void Upsert<T, TId>(T obj, TId id) where T : IHasId<TId>;

        Task<IEnumerable<T>> AllAsync<T>();
        Task<int> CountAsync<T>();
        Task<T> GetAsync<T>(int key) where T : IHasId<int>;
        Task<int> InsertAsync<T>(T obj) where T : IHasId<int>;
        Task<int> DeleteAsync<T>(int key) where T : IHasId<int>;
        Task<int> DeleteAllAsync<T>();
        Task<int> UpdateAsync<T>(T obj) where T : IHasId<int>;
        Task<int> UpsertAsync<T>(T obj) where T : IHasId<int>;
    }
}