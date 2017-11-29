using OrmLite.Model;

namespace OrmLite.Query.Tests
{
    public class Temp : IHasId<int>
    {
        public int Id { get; set; }
        public string Text { get; set; }
    }
}