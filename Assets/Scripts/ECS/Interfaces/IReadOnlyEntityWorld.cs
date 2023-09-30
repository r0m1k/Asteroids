using System;

namespace ECS
{
    public interface IReadOnlyEntityWorld
    {
        IEntity Get(long id);
        TEntity Get<TEntity>(long id) where TEntity : class, IEntity;

        IEntity[] GetAll();
        IEntity[] GetAll(Predicate<IEntity> predicate);

        TEntity FindFirst<TEntity>() where TEntity : class, IEntity;
        TEntity[] Find<TEntity>() where TEntity : class, IEntity;

        bool Visit<TEntity>(Action<IEntity> visitor) where TEntity : class, IEntity;

        EntityFilter CreateFilter();
        
        EntityFilter FilterByComponents<T1>() where T1 : Component;
        EntityFilter FilterByComponents<T1, T2>() where T1 : Component where T2 : Component;
        EntityFilter FilterByComponents<T1, T2, T3>() where T1 : Component where T2 : Component where T3 : Component;
        EntityFilter FilterByComponents<T1, T2, T3, T4>() where T1 : Component where T2 : Component where T3 : Component where T4 : Component;
        EntityFilter FilterByComponents(params Type[] componentsTypes);
    }
}