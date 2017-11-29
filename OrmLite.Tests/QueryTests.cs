using System.Data.SqlClient;
using System.Linq;
using OrmLite.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.OrmLite;
using OrmLite.Model;

namespace OrmLite.Query.Tests
{
    [TestClass]
    public class OrmLiteQueryTests
    {
        [TestInitialize]
        public void Test_Initialize()
        {
            using (var uow = new UnitOfWork())
            {
                uow.Db.CreateTableIfNotExists<Temp>();
                uow.Db.DeleteAll<Temp>();
                for (var i = 1; i < 4; i++)
                    uow.Repository.Insert(new Temp {Id = i, Text = i < 3 ? "A" : "B"});
                
                uow.Query.Execute(@"
                    IF (OBJECT_ID('spTemp', 'P') IS NOT NULL)
                        DROP PROCEDURE spTemp");
                uow.Query.Execute(@"
                    CREATE PROCEDURE spTemp (@MAX INT, @MIN INT = 1, @TEXT VARCHAR(8) = '%') AS
                    BEGIN
                        SELECT * FROM Temp WHERE Id >= @MIN AND Id <= @MAX AND Text LIKE @Text;
                    END");
            }
        }

        [TestCleanup]
        public void Test_Cleanup()
        {
            using (var uow = new UnitOfWork())
            {
                uow.Db.DropTable<Temp>();
                uow.Query.Execute("DROP PROCEDURE spTemp");
            }
        }

        [TestMethod]
        public void Test_GetSql()
        {
            using (var uow = new UnitOfWork())
            {
                var n = uow.Query.Get<Temp>("SELECT * FROM Temp WHERE Id = 1");

                Assert.AreEqual(n.Id, 1);
            }
        }

        [TestMethod]
        public void Test_GetSp()
        {
            using (var uow = new UnitOfWork())
            {
                var n = uow.Query.Get<Temp>("EXEC spTemp @Id = 1");

                Assert.AreEqual(n.Id, 1);
            }
        }

        [TestMethod]
        public void Test_FindSql()
        {
            using (var uow = new UnitOfWork())
            {
                var n = uow.Query.Find<Temp>("SELECT * FROM Temp WHERE Id < 4");

                Assert.AreEqual(n.Count(), 3);
            }
        }

        [TestMethod]
        public void Test_FindSp()
        {
            using (var uow = new UnitOfWork())
            {
                var n = uow.Query.Find<Temp>("EXEC spTemp @Id = 3");

                Assert.AreEqual(n.Count(), 3);
            }
        }

        [TestMethod]
        public void Test_Scalar()
        {
            using (var uow = new UnitOfWork())
            {
                var n = uow.Query.Scalar<int>("SELECT COUNT(*) FROM Temp");

                Assert.AreEqual(n, 3);
            }
        }

        [TestMethod]
        public void Test_GetWithParams()
        {
            using (var uow = new UnitOfWork())
            {
                var n = uow.Query.Get<Temp>("EXEC spTemp @MAX, @MIN, @TEXT", 
                    new SqlParameter("@MIN", 1), new SqlParameter("@MAX", 3), new SqlParameter("@TEXT", "A"));

                Assert.AreEqual(n.Text, "A");
            }
        }

        [TestMethod]
        public void Test_FindWithParams()
        {
            using (var uow = new UnitOfWork())
            {
                var n = uow.Query.Find<Temp>("EXEC spTemp @MAX, @MIN, @TEXT",
                    new SqlParameter("@MIN", 1), new SqlParameter("@MAX", 3), new SqlParameter("@TEXT", "A"));

                Assert.AreEqual(n.Count(), 2);
            }
        }
    }
}