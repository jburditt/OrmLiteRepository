using System;
using System.Collections.Generic;
using OrmLite.Model;

namespace OrmLite.Model.Tests
{
    public static class Data
    {
        public static Product Product = new Product
        {
            Id = 1,
            Description = "TEST",
        };
    }
}