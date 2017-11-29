using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using OrmLite.Model;

namespace OrmLite.Model.Tests
{
    [TestClass]
    public class OrmLiteRepositoryAsyncTests
    {
        private Product product = Data.Product;

        [TestMethod]
        public async Task Test_All()
        {
            using (var uow = new UnitOfWork())
            {
                var n = await uow.Repository.AllAsync<Product>();

                Assert.IsNotNull(n);
            }
        }

        [TestMethod]
        public async Task Test_Count()
        {
            using (var uow = new UnitOfWork())
            {
                var n = await uow.Repository.CountAsync<Product>();

                Assert.IsNotNull(n);
            }
        }

        [TestMethod]
        public async Task Test_Get()
        {
            using (var uow = new UnitOfWork())
            {
                var n = await uow.Repository.GetAsync<Product>(1);

                Assert.IsNotNull(n);
            }
        }

        [TestMethod]
        public async Task Test_GetPredicates()
        {
            using (var uow = new UnitOfWork())
            {
                var n = await uow.Repository.GetAsync<Product>(p => p.Description == "TEST");

                Assert.IsNotNull(n);
            }
        }

        [TestMethod]
        public async Task Test_Find()
        {
            using (var uow = new UnitOfWork())
            {
                var n = await uow.Repository.FindAsync<Product>(p => p.Description == "TEST");

                Assert.IsNotNull(n);
            }
        }

        [TestMethod]
        public async Task Test_InsertNoId()
        {
            using (var uow = new UnitOfWork())
            {
                var n = await uow.Repository.Insert_NoReturnIdAsync(product);

                Assert.IsNotNull(n);
            }
        }

        [TestMethod]
        public async Task Test_Insert()
        {
            using (var uow = new UnitOfWork())
            {
                var n = await uow.Repository.InsertAsync(product);

                Assert.IsNotNull(n);
            }
        }

        [TestMethod]
        public async Task Test_Delete()
        {
            using (var uow = new UnitOfWork())
            {
                var n = await uow.Repository.DeleteAsync<Product>(1);

                Assert.IsTrue(n > 0);
            }
        }

        [TestMethod]
        public async Task Test_DeletePredicates()
        {
            using (var uow = new UnitOfWork())
            {
                var n = await uow.Repository.DeleteAsync<Product>(p => p.Description == "TEST");

                Assert.IsTrue(n > 0);
            }
        }

        [TestMethod]
        public async Task Test_Update()
        {
            using (var uow = new UnitOfWork())
            {
                product.Description = "A";
                var n = await uow.Repository.UpdateAsync(product);

                Assert.IsTrue(n > 0);
            }
        }

        [TestMethod]
        public async Task Test_UpdateById()
        {
            using (var uow = new UnitOfWork())
            {
                var n = await uow.Repository.UpdateAsync<Product>(1, u => u.Description = "B");

                Assert.IsTrue(n > 0);
            }
        }

        [TestMethod]
        public async Task Test_Upsert()
        {
            using (var uow = new UnitOfWork())
            {
                var n = await uow.Repository.UpsertAsync(product);

                Assert.IsTrue(n);
            }
        }
    }
}
