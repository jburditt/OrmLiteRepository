using OrmLite.Model;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OrmLite.Repository
{
    public class OrmLiteRepository : IRepository
    {
        protected IDbConnection Db;

        public OrmLiteRepository(IDbConnection db)
        {
            Db = db;
        }

        #region Sync

        public virtual IEnumerable<T> All<T>()
        {
            return Db.Select<T>();
        }

        public virtual T Get<T>(int id) where T : IHasId<int>
        {
            return Db.SingleById<T>(id);
        }

        public virtual T Get<T>(long id) where T : IHasId<long>
        {
            return Db.SingleById<T>(id);
        }

        public virtual T Get<T>(Expression<Func<T, bool>> predicates)
        {
            return Db.Select(predicates).FirstOrDefault();
        }

        public virtual Tuple<T, TI> GetInclude<T, TI>(Expression<Func<T, TI, bool>> joinOn)
        {
            return Db.SelectMulti<T, TI>(Db.From<T>().LeftJoin<T, TI>(joinOn).Limit(1)).FirstOrDefault();
        }

        public virtual Tuple<T, TI> GetInclude<T, TI>(Expression<Func<T, TI, bool>> joinOn, Expression<Func<T, TI, bool>> predicates)
        {
            return Db.SelectMulti<T, TI>(Db.From<T>().LeftJoin<T, TI>(joinOn).Where(predicates).Limit(1)).FirstOrDefault();
        }

        public virtual Tuple<T, TI> GetInclude<T, TI>(Expression<Func<T, TI, bool>> joinOn, Expression<Func<T, TI, bool>> predicates, Expression<Func<TI, object>> orderBy)
        {
            return Db.SelectMulti<T, TI>(Db.From<T>().LeftJoin<T, TI>(joinOn).Where(predicates).OrderBy(orderBy)).FirstOrDefault();
        }

        public virtual IEnumerable<T> Find<T>(Expression<Func<T, bool>> exp)
        {
            return Db.Select(exp);
        }

        public virtual IEnumerable<Tuple<T, TI>> FindInclude<T, TI>(Expression<Func<T, TI, bool>> joinOn, Expression<Func<T, TI, bool>> predicates)
        {
            return Db.SelectMulti<T, TI>(Db.From<T>().LeftJoin<T, TI>(joinOn).Where(predicates));
        }

        public virtual IEnumerable<Tuple<T, TI>> FindInclude<T, TI>(Expression<Func<T, TI, bool>> joinExpression, Expression<Func<T, TI, bool>> predicates, Expression<Func<TI, object>> orderBy)
        {
            return Db.SelectMulti<T, TI>(Db.From<T>().LeftJoin<T, TI>(joinExpression).Where(predicates).OrderBy(orderBy));
        }

        public virtual void Insert_NoReturnId<T>(T obj)
        {
            Db.Insert(obj, true);
        }

        public virtual int Insert<T>(T obj) where T : IHasId<int>
        {
            Db.Insert(obj, true);
            return obj.Id;
        }

        public virtual TId Insert<T, TId>(T obj, TId id) where T : IHasId<TId>
        {
            Db.Insert(obj, true);
            return obj.Id;
        }

        public virtual int InsertRange<T>(IEnumerable<T> objs)
        {
            var count = 0;

            foreach (var obj in objs)
            {
                Insert_NoReturnId(obj);
                count++;
            }

            return count;
        }

        public virtual int Delete<T>(int id) where T : IHasId<int>
        {
            return Db.DeleteById<T>(id);
        }

        public virtual void Delete<T>(Expression<Func<T, bool>> exp)
        {
            Db.Delete(exp);
        }

        public virtual void DeleteAll<T>()
        {
            Db.DeleteAll<T>();
        }

        public virtual void Update<T>(T obj)
        {
            Db.Update(obj);
        }

        public virtual void Update<T>(int id, params Func<T, object>[] properties)
        {
            var obj = Db.SingleById<T>(id);
            foreach (var lambda in properties)
                lambda.Invoke(obj);
            Db.Update(obj);
        }

        public virtual void Update<T>(long id, params Func<T, object>[] properties)
        {
            var obj = Db.SingleById<T>(id);
            foreach (var lambda in properties)
                lambda.Invoke(obj);
            Db.Update(obj);
        }

        public virtual int Upsert<T>(T obj) where T : IHasId<int>
        {
            Db.Save(obj, true);
            return obj.Id;
        }

        public virtual TId Upsert<T, TId>(TId id, T obj) where T : IHasId<TId>
        {
            Db.Save(obj, true);
            return obj.Id;
        }

        public virtual int Count<T>()
        {
            return (int)Db.Count<T>();
        }

        #endregion Sync

        #region Async

        public virtual async Task<IEnumerable<T>> AllAsync<T>()
        {
            return await Db.SelectAsync<T>();
        }

        public virtual async Task<int> CountAsync<T>()
        {
            return (int)await Db.CountAsync<T>();
        }

        public virtual async Task<T> GetAsync<T>(int Id) where T : IHasId<int>
        {
            return await Db.SingleByIdAsync<T>(Id);
        }

        public virtual async Task<T> GetAsync<T>(Expression<Func<T, bool>> predicates)
        {
            return await Db.SingleByIdAsync<T>(predicates);
        }

        public virtual async Task<Tuple<T, TI>> GetIncludeAsync<T, TI>(Expression<Func<T, TI, bool>> joinOn)
        {
            return (await Db.SelectMultiAsync<T, TI>(Db.From<T>().Join<T, TI>(joinOn).Limit(1))).FirstOrDefault();
        }

        public virtual async Task<Tuple<T, TI>> GetIncludeAsync<T, TI>(Expression<Func<T, TI, bool>> joinOn, Expression<Func<T, bool>> predicates)
        {
            return (await Db.SelectMultiAsync<T, TI>(Db.From<T>().Join<T, TI>(joinOn).Where(predicates).Limit(1))).FirstOrDefault();
        }

        public virtual async Task<Tuple<T, TI>> GetIncludeAsync<T, TI>(Expression<Func<T, TI, bool>> joinOn, Expression<Func<T, bool>> predicates, Expression<Func<TI, object>> orderBy)
        {
            return (await Db.SelectMultiAsync<T, TI>(Db.From<T>().Join<T, TI>(joinOn).Where(predicates).OrderBy(orderBy).Limit(1))).FirstOrDefault();
        }

        public virtual async Task<IEnumerable<T>> FindAsync<T>(Expression<Func<T, bool>> predicates)
        {
            return await Db.SelectAsync(predicates);
        }

        public virtual async Task<IEnumerable<Tuple<T, TI>>> FindIncludeAsync<T, TI>(Expression<Func<T, TI, bool>> joinOn, Expression<Func<T, bool>> predicates)
        {
            return await Db.SelectMultiAsync<T, TI>(Db.From<T>().Join<T, TI>(joinOn).Where(predicates));
        }

        public virtual async Task<bool> Insert_NoReturnIdAsync<T>(T obj)
        {
            return await Db.InsertAsync(obj, true) > 0;
        }

        public virtual async Task<int> InsertAsync<T>(T obj) where T : IHasId<int>
        {
            return (int)await Db.InsertAsync(obj, true);
        }

        public virtual async Task<int> DeleteAsync<T>(int id)
        {
            return await Db.DeleteByIdAsync<T>(id);
        }

        public virtual async Task<int> DeleteAsync<T>(Expression<Func<T, bool>> predicates)
        {
            return await Db.DeleteAsync(predicates);
        }

        public virtual async Task<int> UpdateAsync<T>(T obj)
        {
            return await Db.UpdateAsync(obj);
        }

        public virtual async Task<int> UpdateAsync<T>(int Id, params Func<T, object>[] properties)
        {
            var obj = Db.SingleById<T>(Id);
            foreach (var lambda in properties)
                lambda.Invoke(obj);
            return await Db.UpdateAsync(obj);
        }

        public virtual async Task<bool> UpsertAsync<T>(T obj) where T : IHasId<int>
        {
            return await Db.SaveAsync(obj, true);
        }

        #endregion Async
    }
}