using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using Common;
using OrmLite.TableStorage;
using OrmLite.Model;

namespace OrmLite.Repository
{
    public class OrmLiteTableStorageRepository : ITableStorageRepository
    {
        protected IDbConnection Db;
         
        public OrmLiteTableStorageRepository(IDbConnection db)
        {
            Db = db;
        }

        #region Sync

        public IEnumerable<T> All<T>()
        {
            var tableName = GetAliasName(typeof(T));
            return Db.All<KeyValue>(tableName).Select(item => item.value.Deserialize<T>());
        }

        public int Count<T>()
        {
            return (int)Db.Count<T>();
        }

        public T Get<T>(int key) where T : IHasId<int>
        {
            var tableName = GetAliasName(typeof(T));
            var keyValue = Db.Select<KeyValue>(tableName, q => q.Where(p => p.Id == key)).FirstOrDefault();
            return keyValue?.value == null ? default(T) : keyValue.value.Deserialize<T>();
        }

        public T Get<T>(long key) where T : IHasId<long>
        {
            var tableName = GetAliasName(typeof(T));
            var keyValue = Db.Select<KeyValue>(tableName, q => q.Where(p => p.Id == key)).FirstOrDefault();
            return keyValue?.value == null ? default(T) : keyValue.value.Deserialize<T>();
        }

        // TODO Complete DynamoDb GetLatest and refactor this method.
        public T GetLatest<T>(long key) where T : IHasId<long>
        {
            var tableName = GetAliasName(typeof(T));
            var keyValue = Db.Select<KeyValue>(tableName, q => q.Where(p => p.Id == key).UnsafeOrderBy("LastUpdated, CreatedOn DESC")).FirstOrDefault();
            return keyValue?.value == null ? default(T) : keyValue.value.Deserialize<T>();
        }

        public T Get<T>(Expression<Func<T, bool>> exp)
        {
            throw new NotImplementedException();
        }

        //public T Get<T, TId>(TId key) where T : IHasId<TId>
        //{
        //    var tableName = GetAliasName(typeof(T));
        //    var keyValue = Db.Select<Tuple<TId, string>>(tableName, q => q.Where(p => p.Item1 == key)).FirstOrDefault();
        //    return keyValue?.value == null ? default(T) : keyValue.value.Deserialize<T>();
        //}

        public long Insert<T>(T obj) where T : IHasId<long>
        {
            var tableName = GetAliasName(typeof(T));

            if (obj is IModifiedDate)
            {
                //(obj as IModifiedDate).CreatedOn = DateTime.Now;  // This is what would be used in DynamoDb
                Db.Insert(tableName, new { obj.Id, value = obj.Serialize(), CreatedOn = DateTime.Now });
            }
            else
            {
                Db.Insert(tableName, new { obj.Id, value = obj.Serialize() });
            }

            return obj.Id;
        }

        public void Insert<T, TId>(T obj, TId id) where T : IHasId<TId>
        {
            var tableName = GetAliasName(typeof(T));

            if (obj is IModifiedDate)
            {
                //(obj as IModifiedDate).CreatedOn = DateTime.Now;
                Db.Insert(tableName, new { obj.Id, value = obj.Serialize(), CreatedOn = DateTime.Now });
            }
            else
            {
                Db.Insert(tableName, new { obj.Id, value = obj.Serialize() });
            }
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
            Db.DeleteById<T>(key);
        }

        public int DeleteAll<T>()
        {
            return Db.DeleteAll<T>();
        }

        public void Update<T>(T obj) where T : IHasId<long>
        {
            var tableName = GetAliasName(typeof(T));

            if (obj is IModifiedDate)
            {
                //(obj as IModifiedDate).LastUpdated = DateTime.Now;
                Db.Update(tableName, new { obj.Id, value = obj.Serialize(), LastUpdated = DateTime.Now });
            } else
            {
                Db.Update(tableName, new { obj.Id, value = obj.Serialize() });
            }
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

        //public void Upsert<T, TId>(T obj, TId id) where T : IHasId<TId>
        //{
        //    if (Get<T>(id) != null)
        //        Update(obj);
        //    else
        //        Insert(obj);
        //}

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

        public Task<T> GetAsync<T>(int key) where T : IHasId<int>
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync<T>(T obj) where T : IHasId<int>
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync<T>(int key) where T : IHasId<int>
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAllAsync<T>()
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync<T>(T obj) where T : IHasId<int>
        {
            throw new NotImplementedException();
        }

        public Task<int> UpsertAsync<T>(T obj) where T : IHasId<int>
        {
            throw new NotImplementedException();
        }

        #endregion Async

        #region Helpers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string GetAliasName(MemberInfo type)
        {
            var attribute = (AliasAttribute)Attribute.GetCustomAttribute(type, typeof(AliasAttribute));
            return attribute != null ? attribute.Name : type.Name;
        }

        #endregion Helpers
    }
}