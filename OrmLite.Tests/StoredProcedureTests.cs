using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using OrmLite.Model;
using OrmLite.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.OrmLite;
using StoredProcedures;

namespace OrmLite.Tests
{
    [TestClass]
    public class StoredProcedureTests
    {
        [TestMethod]
        public void Test_GetSpSimple()
        {
            using (var uow = new UnitOfWork())
            {
                //var countryList = uow.Db.Stored_Proc().ConvertToList<dynamic>();
                
                //Assert.AreEqual(countryList[0].Name, "Canada");
            }
        }

        public static async Task<List<Product>> Get_Async_Stored_Procedure(IDbConnection db)
        {
            IDbDataParameter returnValue;
            return await db.SqlListAsync<Product>("Stored_Proc", cmd =>
            {
                cmd.CommandType = CommandType.StoredProcedure;
                returnValue = cmd.AddParam("__ReturnValue", 0, ParameterDirection.ReturnValue, DbType.Int32);
            });
        }
    }
}