namespace ECS
{
    public interface IEntitySystemRequireStart : IEntitySystem
    {
        void WorldStarted();
    }
}