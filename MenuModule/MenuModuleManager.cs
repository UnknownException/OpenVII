using Shared.Engine;
using Shared.Modules;

namespace MenuModule
{
    public class MenuModuleManager : IMenuModuleManager
    {
        private readonly IEngineManager _engineManager;

        public MenuModuleManager(IEngineManager engineManager)
        {
            _engineManager = engineManager;
        }

        public void OnDestroy()
        {
            throw new System.NotImplementedException();
        }

        public void OnDraw()
        {
            throw new System.NotImplementedException();
        }

        public void OnUpdate(long deltaTime)
        {
            throw new System.NotImplementedException();
        }

        public void Register()
        {
            throw new System.NotImplementedException();
        }
    }
}
