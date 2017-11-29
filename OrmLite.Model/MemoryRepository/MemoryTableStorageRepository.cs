using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OrmLite.Model
{
    public class MemoryTableStorageRepository : ITableStorageRepository
    {
        private readonly ConcurrentDictionary<Type, List<object>> _db;

        public MemoryTableStorageRepository()
        {
            _db = new ConcurrentDictionary<Type, List<object>>();
        }

        public MemoryTableStorageRepository(ConcurrentDictionary<Type, List<object>> db)
        {
            _db = db;
        }

        #region Sync

        public IEnumerable<T> All<T>()
        {
            return _db[typeof(T)].OfType<T>();
        }

        public int Count<T>()
        {
            return _db[typeof(T)].Count();
        }       

        public T Get<T>(long key) where T : IHasId<long>
        {
            // check list exist
            if (!_db.ContainsKey(typeof(T)))
                return default(T);

            return _db[typeof(T)].OfType<T>().FirstOrDefault(n => n.Id == key);
        }

        public T Get<T>(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public T GetLatest<T>(long key) where T : IHasId<long>
        {
            throw new NotImplementedException();
        }

        public long Insert<T>(T obj) where T : IHasId<long>
        {
            // instantiate if list does not exist for this object type
            if (!_db.ContainsKey(typeof(T)))
                _db[typeof(T)] = new List<object>();

            // if object has no id assigned already, generate a new id
            if (obj.Id == default(int))
            {
                // get id
                var id = GetId<T>() + 1;

                // add object to list
                obj.Id = id;
            }

            _db[typeof(T)].Add(obj);

            return obj.Id;
        }

        public int InsertRange<T>(IEnumerable<T> objs) where T : IHasId<long>
        {
            var count = 0;

            foreach (var obj in objs)
            {
                Insert(obj);
                count++;
            }

            return count;
        }

        public void Delete<T>(long key) where T : IHasId<long>
        {
            // check list exist
            if (!_db.ContainsKey(typeof(T)))
                return;

            // find object with matching id
            for (var i = 0; i < _db[typeof(T)].Count; i++)
                if (_db[typeof(T)].OfType<T>().ToList()[i].Id == key)
                {
                    _db[typeof(T)].RemoveAt(i);
                    return;
                }

            // object not found
            return;
        }

        public int DeleteAll<T>()
        {
            _db[typeof(T)] = new List<object>();
            return 0; // TODO return amount of items deleted
        }

        public void Update<T>(T obj) where T : IHasId<long>
        {
            // check list exist
            if (!_db.ContainsKey(typeof(T)))
                return;

            // find object with matching id
            for (var i = 0; i < _db[typeof(T)].Count; i++)
                if (_db[typeof(T)].OfType<T>().ToList()[i].Id == obj.Id)
                {
                    _db[typeof(T)][i] = obj;
                    return;
                }

            // object not found
            return;
        }

        public void Update<T>(long Id, params Func<T, object>[] properties)
        {
            throw new NotImplementedException();
        }

        public long Upsert<T>(T obj) where T : IHasId<long>
        {
            if (Get<T>(obj.Id) != null)
                Update(obj);
            else
                Insert(obj);

            return obj.Id;
        }

        #endregion Sync

        #region Async

        public virtual async Task<IEnumerable<T>> AllAsync<T>()
        {
            throw new NotImplementedException();
        }

        public virtual async Task<int> CountAsync<T>()
        {
            throw new NotImplementedException();
        }

        public virtual async Task<T> GetAsync<T>(int key) where T : IHasId<int>
        {
            throw new NotImplementedException();
        }

        public virtual async Task<int> InsertAsync<T>(T obj) where T : IHasId<int>
        {
            throw new NotImplementedException();
        }

        public virtual async Task<int> DeleteAsync<T>(int key) where T : IHasId<int>
        {
            throw new NotImplementedException();
        }

        public virtual async Task<int> DeleteAllAsync<T>()
        {
            throw new NotImplementedException();
        }

        public virtual async Task<int> UpdateAsync<T>(T obj) where T : IHasId<int>
        {
            throw new NotImplementedException();
        }

        public virtual async Task<int> UpsertAsync<T>(T obj) where T : IHasId<int>
        {
            throw new NotImplementedException();
        }

        #endregion Async

        #region Helper methods

        private long GetId<T>() where T : IHasId<long>
        {
            //return new Guid().ToString();
            return _db[typeof(T)].Count == 0 ? 0 : _db[typeof(T)].OfType<T>().Last().Id;
        }

        #endregion
    }
}