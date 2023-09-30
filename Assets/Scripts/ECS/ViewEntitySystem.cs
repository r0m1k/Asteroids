namespace ECS
{
    public class ViewEntitySystem : EntitySystem, IEntitySystemRequireEntityViewWorld
    {
        public IReadOnlyEntityViewWorld ViewWorld { get; set; }
    }
}