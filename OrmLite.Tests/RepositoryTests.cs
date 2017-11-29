using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using OrmLite.Model;
using ServiceStack.OrmLite;
using System.Configuration;
using OrmLite.Model.Tests;

namespace OrmLite.Repository.Tests
{
    /// <summary>
    /// ORM Lite Repository CRUD and SP Tests
    /// </summary>
    /// 
    /// TODO
    /// Research Database connection pooling and lifetime scope
    /// - Nuget deploy from Jenkins
    /// - DeleteAll, Delete w/IdParams, Count w/SqlParams?
    /// - MockRepo, MockQuery
    /// - rewrite TestCleanup
    /// - Test_TooManyCharacters
    /// - Test_InsertInclude

    [TestClass]
    public class OrmLiteRepositoryTests
    {
        private IUnitOfWork _unitOfWork;

        private Product product = Data.Product;

        [TestInitialize]
        [TestMethod]
        public void Test_Initialize()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

            _unitOfWork = new UnitOfWork(connectionString);

            using (_unitOfWork.Open())
            {
                _unitOfWork.Repository.Upsert(product);
            }
        }

        [TestCleanup]
        public void Test_Cleanup()
        {
            using (var uow = new UnitOfWork())
            {
                uow.Db.Delete(uow.Db.From<Product>().Where(p => Sql.In(p.Id, 1, 2, 3)));
            }
        }

        [TestMethod]
        public void Test_Insert()
        {
            using (var uow = new UnitOfWork())
            {
                uow.Repository.Insert_NoReturnId(product);

                var n = uow.Repository.Get<Product>(p => p.Description == "TEST");

                Assert.IsNotNull(n);
            }
        }

        [TestMethod]
        public void Test_InsertInclude()
        {
        }

        [TestMethod]
        public void Test_InsertRange()
        {
            using (var uow = new UnitOfWork())
            {
                var product2 = product;
                product2.Id = 2;
                var product3 = product;
                product3.Id = 3;
                var products = new List<Product> { product, product2, product3 };

                var count = uow.Repository.Count<Product>();

                uow.Repository.InsertRange(products);

                Assert.AreEqual(count + 3, uow.Repository.Count<Product>());
            }
        }

        [TestMethod]
        public void Test_Upsert()
        {
            using (var uow = new UnitOfWork())
            {
                product.Id = 2;
                product.Description = "TEST 111";
                uow.Repository.Upsert(product);
                uow.Repository.Upsert(product);
                uow.Repository.Upsert(product);
                product.Id = 3;
                product.Description = "TEST 222";
                uow.Repository.Upsert(product);
                uow.Repository.Upsert(product);

                var n = uow.Repository.Count<Product>();

                Assert.AreEqual(n, 632);
            }
        }

        [TestMethod]
        public void Test_GetById()
        {
            using (var uow = new UnitOfWork())
            {
                var n = uow.Repository.Get<Product>(1);

                Assert.AreEqual(n.Description, "TEST");
            }
        }

        [TestMethod]
        public void Test_UpdatePartial()
        {
            using (var uow = new UnitOfWork())
            {
                uow.Repository.Update<Product>(1, p => p.Description = "A");

                var n = uow.Repository.Get<Product>(1);

                Assert.AreEqual(n.Description, "A");
            }
        }

        [TestMethod]
        public void Test_Delete()
        {
            using (var uow = new UnitOfWork())
            {
                uow.Repository.Delete<Product>(1);

                var n = uow.Repository.Get<Product>(1);

                Assert.IsNull(n);
            }
        }

        [TestMethod]
        public void Test_GetInclude()
        {
            //using (var uow = new UnitOfWork())
            //{
            //    var cartItems = uow.Repository.GetInclude<Cart, CartItem>((j1, j2) => j1.CartId == j2.CartId, (cart, item) => cart.CartId == 1000, o => o.CreatedDTS);

            //    Assert.IsNotNull(cartItems);
            //}
        }

        [TestMethod]
        public void GetInclude_No_Items()
        {
            //using (var uow = new UnitOfWork())
            //{
            //    var cartItems = uow.Repository.GetInclude<Cart, CartItem>((cart, cartItem) => cart.CartId == cartItem.CartId, (cart, item) => cart.CartId == 1001, o => o.CreatedDTS);

            //    Assert.IsNotNull(cartItems);
            //}
        }

        [TestMethod]
        public void Get_Composite_Object()
        {
            //using (var uow = new UnitOfWork())
            //{
            //    var cart = uow.Repository
            //        .GetInclude<Cart, CartItem>((j1, j2) => j1.CartId == j2.CartId, (c, item) => c.CartId == 1000, o => o.CreatedDTS)
            //        .ToComposite<CartComposite, Cart, CartItem>();

            //    Assert.IsNotNull(cart);
            //}
        }

        [TestMethod]
        public void Test_FindInclude()
        {
            //using (_unitOfWork.Open())
            //{
            //    var cartItems = _unitOfWork.Repository.FindInclude<Cart, CartItem>((j1, j2) => j1.CartId == j2.CartId, (x1, x2) => x1.CartId == 1000, o => o.CreatedDTS);

            //    Assert.AreEqual(3, cartItems.Count());
            //}
        }

        [TestMethod]
        public void Find_Composite_Object()
        {
            //using (var uow = new UnitOfWork())
            //{
            //    var cart = uow.Repository
            //        .FindInclude<Cart, CartItem>((j1, j2) => j1.CartId == j2.CartId, null, o => o.CreatedDTS)
            //        .ToComposite<CartComposite, Cart, CartItem>();

            //    Assert.IsNotNull(cart);
            //}
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "ExecuteReader requires an open and available Connection. The connection's current state is closed.")]
        public void Test_DatabaseScope()
        {
            var uow = new UnitOfWork();

            using (uow = new UnitOfWork())
            {
                uow.Repository.Get<Product>(1);
            }

            uow.Repository.Get<Product>(1);
        }
    }
}