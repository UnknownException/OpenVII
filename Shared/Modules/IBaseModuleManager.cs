namespace Shared.Modules
{
    public interface IBaseModuleManager
    {
        void Register();
        void OnUpdate(long deltaTime);
        void OnDraw();
        void OnDestroy();
    }
}
