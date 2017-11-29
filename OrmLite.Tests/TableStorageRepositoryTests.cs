using System;
using System.Collections.Generic;
using OrmLite.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using OrmLite.Model;

namespace OrmLite.Tests
{
    /// <summary>
    /// WARNING: Running any of these tests will delete all of your Order table
    /// TODO Create a new table and use that table instead
    /// CREATE TABLE [dbo].[TestOrder](
    //	[Id]
    //    [bigint]
    //    NOT NULL,

    //    [value] [varchar](max) NOT NULL,
    //CONSTRAINT[PK_TestOrder] PRIMARY KEY CLUSTERED
    //(
    //[Id] ASC
    //)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]
    //) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]
    //GO
    /// </summary>
    [TestClass]
    public class TableStorageRepositoryTests
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

        [TestInitialize]
        public void Test_Initialize()
        {
            //using (var uow = new UnitOfWork())
            //{
            //    uow.Db.DropAndCreateTable<KeyValue>("TestOrder");
            //    uow.TableStorageRepository.DeleteAll<OrderModel>();
            //}
        }

        [TestMethod]
        public void Test_Insert()
        {
            using (var uow = new UnitOfWork())
            {
                uow.TableStorageRepository.Insert(order);

                var n = uow.TableStorageRepository.Get<Order>(order.Id);

                Assert.IsNotNull(n);
            }
        }

        [TestMethod]
        public void Test_InsertRange()
        {
            using (var uow = new UnitOfWork())
            {
                var order2 = Data.Order;
                order2.Id = 2;
                var order3 = Data.Order;
                order3.Id = 3;
                var orders = new List<Order> { Data.Order, order2, order3 };

                var count = uow.TableStorageRepository.Count<Order>();

                uow.TableStorageRepository.InsertRange(orders);

                Assert.AreEqual(count + 3, uow.TableStorageRepository.Count<Order>());
            }
        }

        [TestMethod]
        public void Test_Upsert()
        {
            using (var uow = new UnitOfWork())
            {
                uow.TableStorageRepository.Upsert(order);

                var n = uow.TableStorageRepository.Get<Order>(order.Id);

                Assert.IsNotNull(n);

                uow.TableStorageRepository.Upsert(order);
                uow.TableStorageRepository.Upsert(order);

                Assert.AreEqual(1, uow.TableStorageRepository.Count<Order>());
            }
        }

        [TestMethod]
        //[Ignore]
        public void Test_All()
        {
            using (var uow = new UnitOfWork())
            {
                uow.TableStorageRepository.Upsert(order);
                order.Id = 2;
                uow.TableStorageRepository.Upsert(order);
                order.Id = 3;
                uow.TableStorageRepository.Upsert(order);

                var n = uow.TableStorageRepository.All<Order>();

                Assert.AreEqual(3, uow.TableStorageRepository.Count<Order>());
            }
        }

        [TestMethod]
        public void Test_Delete()
        {
            using (var uow = new UnitOfWork())
            {
                uow.TableStorageRepository.Insert(order);
                uow.TableStorageRepository.Delete<Order>(order.Id);

                Assert.AreEqual(0, uow.TableStorageRepository.Count<Order>());
            }
        }

        [TestMethod]
        public void Test_GetLatest()
        {
            using (var uow = new UnitOfWork())
            {
                //uow.TableStorageRepository.DeleteAll<Order>();

                //var order = new Order { Id = 1,  };
                //uow.TableStorageRepository.Insert(archiveOrder);
                //uow.TableStorageRepository.Update(archiveOrder);
                //order.Id = 2;
                //uow.TableStorageRepository.Insert(archiveOrder);
                //uow.TableStorageRepository.Update(archiveOrder);
                //order.Id = 3;
                //uow.TableStorageRepository.Insert(archiveOrder);
                //uow.TableStorageRepository.Update(archiveOrder);
                //order.Id = 4;
                //uow.TableStorageRepository.Insert(archiveOrder);
                //uow.TableStorageRepository.Update(archiveOrder);
                //order.Id = 5;
                //uow.TableStorageRepository.Insert(archiveOrder);
                //uow.TableStorageRepository.Update(archiveOrder);

                //uow.Query.Execute("UPDATE Order SET Id = 1");

                //Assert.AreEqual(5, uow.TableStorageRepository.Count<Order>());

                var lastArchiveOrder = uow.TableStorageRepository.GetLatest<Order>(1481465814824521181);

                Assert.AreEqual(DateTime.Parse("2017-04-03 12:50:30.747"), lastArchiveOrder.Archived);
            }
        }

        [TestMethod]
        public void Test_Temp()
        {
            using (var uow = new UnitOfWork())
            {
                uow.TableStorageRepository.Insert(Data.Order);

                var n = uow.TableStorageRepository.Get<Order>(Data.Order.Id);

                Assert.IsNotNull(n);
            }
        }
    }
}