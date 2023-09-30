namespace ECS
{
    // IEntitySystem is hack to add similarities
    public interface IEntitySystemRequireFixedUpdate : IEntitySystem
    {
        void FixedUpdate(float fixedDeltaTime);
    }
}