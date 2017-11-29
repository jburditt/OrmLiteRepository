using System;
using ServiceStack.DataAnnotations;
using OrmLite.Model;

namespace OrmLite.Model.Tests
{
    [Alias("Product")]
    public partial class Product : IHasId<int>
    {
        [Alias("ProductNo")]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
