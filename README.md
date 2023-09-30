# Asteroids
Starting point
---
Startup and only one scene in project are located at `Assets/Scenes/Bootstrap.unity` with alone `Assets/Content/Prefabs/Misc/Bootstrapper.prefab` prefab with just `Bootstrapper` attached script.

`Scripts` folder structure
---
Game specific logic placed into `Assets/Scripts/Asteroids`. While more non-game code near it.
I isolate few non-game parts: `Infrastructure`, `StateMachine`, `ServicesContainer` and `ECS`.

Infrastructure
---
Contain few helper data structures.
`MinMaxIntRange` and `MinMaxFloatRange`. `ITypeList<>` and `ITypeDictionary<>` with implementations.

ServicesContainer
---
`ServicesContainer` can be (and is) initialized with `ServicesConfiguration`.
It has editor but with some limitation: services ctor parameters are stored in configuration and not expect for changes.

ECS
---
ECS is simple reference type based solution and is start up point for implementation.
It has MANY LINQ usages: it is solution for first iteration with clear and fast write code.
And as reason it has many memory allocations in each frame.

`Asteroids` folder
---
Contain all game specific parts: game states, services interfaces and implementations, ECS (entity and views), UI and data for it.
It split into assemblies to more strict relations and compile time, but not to be as building bricks.
I try to keep all parts be as configurable as can, as a result too many ScriptableObject parameters with little mess.

Data folders
---
Core folders are: `Assets/Content/Data` and `Assets/Content/Prefabs`.
Them contain game configurations/parameters and services/entity/ui prefab correspondently.

Known issue
---
* Error: `Type 'AxisComposite' registered as '1DAxis' (mentioned in '1DAxis(mode=2)') has no public field called 'mode'`
