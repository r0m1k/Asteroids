namespace ECS
{
    // IEntitySystem is hack to add similarities
    public interface IEntitySystemRequireLateUpdate : IEntitySystem
    {
        void LateUpdate(float deltaTime);
    }
}