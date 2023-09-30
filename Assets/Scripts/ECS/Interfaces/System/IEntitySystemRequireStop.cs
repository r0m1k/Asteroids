namespace ECS
{
    public interface IEntitySystemRequireStop : IEntitySystem
    {
        void WorldStopped();
    }
}