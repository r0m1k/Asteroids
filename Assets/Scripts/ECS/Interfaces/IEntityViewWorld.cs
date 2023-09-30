namespace ECS
{
    public interface IEntityViewWorld : IReadOnlyEntityViewWorld
    {
        void Add(IEntityView view);
        void Remove(long id);
    }
}