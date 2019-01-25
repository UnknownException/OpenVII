using Shared.Engine;
using Shared.Modules;

namespace WorldModule
{
    public class WorldModuleManager : IWorldModuleManager
    {
        private readonly IEngineManager _engineManager;
        private readonly IMenuModuleManager _menuModule;
        private readonly IBattleModuleManager _battleModule;

        public WorldModuleManager(IEngineManager engineManager, IMenuModuleManager menuModule, IBattleModuleManager battleModule)
        {
            _engineManager = engineManager;
            _menuModule = menuModule;
            _battleModule = battleModule;
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
