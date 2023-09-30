namespace ECS
{
    public interface IEntitySystem
    {
        IReadOnlyEntityWorld World { get; set; }
    }
}