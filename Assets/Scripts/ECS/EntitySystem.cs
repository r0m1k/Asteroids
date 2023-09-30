namespace ECS
{
    public class EntitySystem : IEntitySystem
    {
        public IReadOnlyEntityWorld World { get; set; }
    }
}