using Shared.Engine;
using Shared.Modules;

namespace MiniGameModule
{
    public class MiniGameModuleManager : IMiniGameModuleManager
    {
        private readonly IEngineManager _engineManager;

        public MiniGameModuleManager(IEngineManager engineManager)
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
