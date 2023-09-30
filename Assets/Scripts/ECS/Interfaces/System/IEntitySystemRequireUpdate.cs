namespace ECS
{
    // IEntitySystem is hack to add similarities
    public interface IEntitySystemRequireUpdate : IEntitySystem
    {
        void Update(float deltaTime);
    }
}