using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using OrmLite.Model;

namespace OrmLite.Model.Tests
{
    [TestClass]
    public class MemoryRepositoryTests
    {
        #region Declarations

        private readonly UnitOfWork uow = new UnitOfWork();
        private IRepository Repo => uow.Repository;
        private Product product = Data.Product;

        #endregion Declarations

        [TestInitialize]
        public void TestInitialize()
        {
            Repo.Insert(product);
        }

        [TestMethod]
        public void Test_All()
        {
            product.Id = 2;
            Repo.Insert(product);
            product.Id = 3;
            Repo.Insert(product);

            var n = Repo.All<Product>();
            
            Assert.AreEqual(3, n.Count());
            Assert.AreEqual(3, Repo.Count<Product>());
        }

        [TestMethod]
        public void Test_Insert()
        {
            var n = Repo.Get<Product>(1);

            Assert.IsNotNull(n);
        }

        [TestMethod]
        public void Test_Get()
        {
            var n = Repo.Get<Product>(2);

            Assert.IsNull(n);
        }

        [TestMethod]
        public void Test_GetPredicate()
        {
            var n = Repo.Get<Product>(p => p.Description == "TEST");

            Assert.AreEqual(n.Description, "TEST");
        }

        [TestMethod]
        public void Test_UpdateObject()
        {
            product.Description = "A";

            Repo.Update(product);
            var n = Repo.Get<Product>(product.Id);

            Assert.AreEqual(n.Description, "A");
        }

        [TestMethod]
        public void Test_Upsert()
        {
            Repo.DeleteAll<Product>();
            Repo.Upsert(product);

            Assert.AreEqual(1, Repo.Count<Product>());

            product.Id = 2;
            product.Description = "B";
            Repo.Upsert(product);
            var n = Repo.Get<Product>(2);

            Assert.AreEqual(1, Repo.Count<Product>());
            Assert.AreEqual("B", n.Description);
        }

        [TestMethod]
        public void Test_Delete()
        {
            var n = Repo.Get<Product>(product.Id);
            Assert.IsNotNull(n);

            Repo.Delete<Product>(product.Id);
            n = Repo.Get<Product>(product.Id);

            Assert.IsNull(n);
        }
    }
}
