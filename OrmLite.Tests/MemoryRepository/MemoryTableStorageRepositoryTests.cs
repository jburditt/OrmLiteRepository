using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrmLite.Model;

namespace OrmLite.Model.Tests
{
    [TestClass]
    public class MemoryTableStorageRepositoryTests
    {
        #region Declarations

        private readonly UnitOfWork uow = new UnitOfWork();
        private IRepository Repo => uow.Repository;
        private Product product = Data.Product;

        #endregion Declarations

        [TestInitialize]
        public void Test_Initialize()
        {
            Repo.Insert(product);
        }

        [TestMethod]
        public void Test_Insert()
        {
            var n = Repo.Get<Product>(product.Id);

            Assert.IsNotNull(n);
        }

        [TestMethod]
        public void Test_Delete()
        {
            Repo.Delete<Product>(product.Id);

            var n = Repo.Get<Product>(product.Id);

            Assert.IsNull(n);
        }

        [TestMethod]
        public void Test_Upsert()
        {
            Repo.Upsert(product);
            Repo.Upsert(product);

            Assert.AreEqual(1, Repo.Count<Product>());
        }
    }
}