namespace ECS
{
    public interface IEntitySystemRequireEntityViewWorld
    {
        IReadOnlyEntityViewWorld ViewWorld { get; set; }
    }
}