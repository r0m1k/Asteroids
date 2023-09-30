using Asteroids.Services;

namespace Asteroids.UI
{
    public interface IUIView
    {
        void Initialize(IUIViewRoot uiRoot, IReadOnlyDataService dataService);
    }
}