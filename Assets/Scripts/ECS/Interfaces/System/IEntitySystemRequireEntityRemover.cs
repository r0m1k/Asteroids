namespace ECS
{
    public interface IEntitySystemRequireEntityRemover
    {
        IEntityRemover EntityRemover { get; set; }
    }
}