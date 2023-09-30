using System;

namespace ECS
{
    // Is this interface need or can be just Entity class?
    // because Component is not need interface in any way
    // for now it is line only contract for Entity class
    public interface IEntity
    {
        long Id { get; }

        T AddComponent<T>() where T : Component, new();
        T AddComponent<T>(T instance) where T : Component;

        T AddComponentIfNotExists<T>() where T : Component, new();

        bool HasComponent<T>() where T : Component;
        bool HasComponent(Type type);

        T GetComponent<T>() where T : Component;
        T GetOrCreateComponent<T>() where T : Component, new();

        bool RemoveComponent<T>() where T : Component;
    }
}