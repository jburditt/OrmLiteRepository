namespace OrmLite.Model
{
    public interface IHasId<T>
    {
        T Id { get; set; }
    }
}