using OrmLite.Model;
using OrmLite.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.OrmLite;
using System.Configuration;

namespace OrmLite.Tests
{
    [TestClass]
    public class ConnectionTests
    {
        private IUnitOfWork _unitOfWork;

        [TestInitialize]
        public void Initialize()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;

            _unitOfWork = new UnitOfWork(connectionString);
        }

        [TestMethod]
        public void Load()
        {
            for (var i = 0; i < 10000; i++)
            {
                using (_unitOfWork.Open())
                {
                }
            }
        }
    }
}