using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OrmLite.Model
{
    // TODO Change IIdentifier.Id to object instead of int e.g. Guid
    // To use a GUID for Id we need GetId() to generate a new Guid. Change name to GetNextKey()
    public class MemoryRepository : IRepository
    {
        private readonly ConcurrentDictionary<Type, List<object>> _db;

        public MemoryRepository()
        {
            _db = new ConcurrentDictionary<Type, List<object>>();
        }

        public MemoryRepository(ConcurrentDictionary<Type, List<object>> db)
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

        // TODO OrmLite and DynamoDb do not have restriction 'where T : IHasId<int>'
        public T Get<T>(int id) where T : IHasId<int>
        {
            // check list exist
            if (!_db.ContainsKey(typeof(T)))
                return default(T);

            return _db[typeof(T)].OfType<T>().FirstOrDefault(n => n.Id == id);
        }

        public T Get<T>(Expression<Func<T, bool>> predicates)
        {
            // check list exist
            if (!_db.ContainsKey(typeof(T)))
                return default(T);

            return _db[typeof(T)].OfType<T>().AsQueryable().Where(predicates).FirstOrDefault();
        }

        public Tuple<T, TI> GetInclude<T, TI>(Expression<Func<T, TI, bool>> joinOn)
        {
            throw new NotImplementedException();
        }

        public Tuple<T, TI> GetInclude<T, TI>(Expression<Func<T, TI, bool>> joinOn, Expression<Func<T, TI, bool>> predicates)
        {
            throw new NotImplementedException();
        }

        public Tuple<T, TI> GetInclude<T, TI>(Expression<Func<T, TI, bool>> joinOn, Expression<Func<T, TI, bool>> predicates, Expression<Func<TI, object>> orderBy)
        {
            throw new NotImplementedException();
        }

        public TId Insert<T, TId>(T obj) where T : IHasId<TId>
        {
            throw new NotImplementedException();
        }

        public void Insert_NoReturnId<T>(T obj)
        {
            throw new NotImplementedException();
        }

        public int Insert<T>(T obj) where T : IHasId<int>
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

        public int InsertRange<T>(IEnumerable<T> objs)
        {
            throw new NotImplementedException();
        }

        public void Update<T>(T obj)
        {
            throw new NotImplementedException();

            // check list exist
            if (!_db.ContainsKey(typeof(T)))
                return;

            // find object with matching id
            //for (var i = 0; i < _db[typeof(T)].Count; i++)
            //    if (_db[typeof(T)].OfType<T>().ToList()[i].Id == obj.Id)
            //    {
            //        _db[typeof(T)][i] = obj;
            //        return;
            //    }

            // object not found
            return;
        }

        public void Update<T>(int id, params Func<T, object>[] properties)
        {
            throw new NotImplementedException();

            // check list exist
            if (!_db.ContainsKey(typeof(T)))
                return;

            // get the existing object
            //var obj = Get<T>(id);

            // perform the changes
            //foreach (var lambda in properties)
            //    lambda.Invoke(obj);

            // save the new object
            //Update(obj);

            return;
        }

        public void Update<T>(long id, params Func<T, object>[] properties)
        {
            throw new NotImplementedException();
        }

        public int Delete<T>(int id) where T : IHasId<int>
        {
            // check list exist
            if (!_db.ContainsKey(typeof(T)))
                return default(int);

            // find object with matching id
            for (var i = 0; i < _db[typeof(T)].Count; i++)
                if (_db[typeof(T)].OfType<T>().ToList()[i].Id == id)
                {
                    _db[typeof(T)].RemoveAt(i);
                    return id;
                }

            // object not found
            return 0;
        }

        public void Delete<T>(Expression<Func<T, bool>> predicates)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll<T>()
        {
            _db[typeof(T)] = new List<object>();
        }

        public IEnumerable<T> Find<T>(Expression<Func<T, bool>> predicate)
        {
            // check list exist
            if (!_db.ContainsKey(typeof(T)))
                return null;

            return _db[typeof(T)].OfType<T>().AsQueryable().Where(predicate);
        }

        public IEnumerable<Tuple<T, TI>> FindInclude<T, TI>(Expression<Func<T, TI, bool>> joinOn, Expression<Func<T, TI, bool>> predicates)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Tuple<T, TI>> FindInclude<T, TI>(Expression<Func<T, TI, bool>> joinOn, Expression<Func<T, TI, bool>> predicates, Expression<Func<TI, object>> orderBy)
        {
            throw new NotImplementedException();
        }

        public int Upsert<T>(T obj) where T : IHasId<int>
        {
            if (Get<T>(obj.Id) != null)
            {
                Update(obj);
                return 1;
            } else 
                return Insert(obj);
        }

        public TId Upsert<T, TId>(TId id, T obj) where T : IHasId<TId>
        {
            throw new NotImplementedException();
        }

        #endregion Sync

        #region Async

        public Task<IEnumerable<T>> AllAsync<T>()
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync<T>()
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync<T>(int id) where T : IHasId<int>
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync<T>(Expression<Func<T, bool>> predicates)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<T, TI>> GetIncludeAsync<T, TI>(Expression<Func<T, TI, bool>> joinOn)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<T, TI>> GetIncludeAsync<T, TI>(Expression<Func<T, TI, bool>> joinOn, Expression<Func<T, bool>> predicates)
        {
            throw new NotImplementedException();
        }

        public Task<Tuple<T, TI>> GetIncludeAsync<T, TI>(Expression<Func<T, TI, bool>> joinOn, Expression<Func<T, bool>> predicates, Expression<Func<TI, object>> orderBy)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> FindAsync<T>(Expression<Func<T, bool>> predicates)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tuple<T, TI>>> FindIncludeAsync<T, TI>(Expression<Func<T, TI, bool>> joinOn, Expression<Func<T, bool>> predicates)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Insert_NoReturnIdAsync<T>(T obj)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync<T>(T obj) where T : IHasId<int>
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync<T>(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync<T>(Expression<Func<T, bool>> predicates)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync<T>(T obj)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync<T>(int Id, params Func<T, object>[] properties)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpsertAsync<T>(T obj) where T : IHasId<int>
        {
            throw new NotImplementedException();
        }

        #endregion Async

        #region Helper methods

        private int GetId<T>() where T : IHasId<int>
        {
            return _db[typeof(T)].Count == 0 ? 0 : _db[typeof(T)].OfType<T>().Last().Id;
        }

        #endregion
    }
}
