using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.Legacy;

namespace OrmLite.TableStorage
{
    /// <summary>
    /// Change alias at runtime e.g. map to one object but save to a different table
    /// </summary>
    /// http://stackoverflow.com/questions/24955640/inject-tablename-as-parameter-for-update-insert-on-genericentity-in-servicesta/24967751#24967751
    public static class GenericTableExtensions
    {
        static object ExecWithAlias<T>(string table, Func<object> fn)
        {
            var modelDef = typeof(T).GetModelMetadata();
            lock (modelDef)
            {
                var hold = modelDef.Alias;
                try
                {
                    modelDef.Alias = table;
                    return fn();
                }
                finally
                {
                    modelDef.Alias = hold;
                }
            }
        }

        public static void DropAndCreateTable<T>(this IDbConnection db, string table)
        {
            ExecWithAlias<T>(table, () => { db.DropAndCreateTable<T>(); return null; });
        }

        public static long Insert<T>(this IDbConnection db, string table, T obj, bool selectIdentity = false)
        {
            return (long)ExecWithAlias<T>(table, () => db.Insert(obj, selectIdentity));
        }

        public static List<T> All<T>(this IDbConnection db, string table)
        {
            return (List<T>)ExecWithAlias<T>(table, db.Select<T>);
        }

        public static List<T> Select<T>(this IDbConnection db, string table, Func<SqlExpression<T>, SqlExpression<T>> expression)
        {
            return (List<T>)ExecWithAlias<T>(table, () => db.Select(expression));
        }

        public static int Update<T>(this IDbConnection db, string table, T item)
        {
            return (int)ExecWithAlias<T>(table, () => db.Update(item));
        }
    }
}