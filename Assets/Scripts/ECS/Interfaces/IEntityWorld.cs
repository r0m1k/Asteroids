namespace ECS
{
    public interface IEntityWorld : IReadOnlyEntityWorld
    {
        // ToDo: long is annoying, may be need some EntityKey struct?
        long Add(IEntity entity);
        bool Remove(long id);
    }
}