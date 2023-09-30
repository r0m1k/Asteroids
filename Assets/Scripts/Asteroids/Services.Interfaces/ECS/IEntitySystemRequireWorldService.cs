namespace Asteroids.Services.ECS
{
    public interface IEntitySystemRequireWorldService
    {
        IEntityWorldService EntityWorldService { get; set; }
    }
}