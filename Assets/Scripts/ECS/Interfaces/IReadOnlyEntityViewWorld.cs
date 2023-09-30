namespace ECS
{
    public interface IReadOnlyEntityViewWorld
    {
        IEntityView Get(long id);
        TEntityView Get<TEntityView>(long id) where TEntityView : class, IEntityView;
    }
}