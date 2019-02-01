using Shared.Engine;
using Shared.Modules;

namespace FieldModule
{
    public class FieldModuleManager : IFieldModuleManager
    {
        private readonly IEngineManager _engineManager;
        private readonly IMenuModuleManager _menuModule;
        private readonly IBattleModuleManager _battleModule;

        public FieldModuleManager(IEngineManager engineManager, IMenuModuleManager menuModule, IBattleModuleManager battleModule)
        {
            _engineManager = engineManager;
            _menuModule = menuModule;
            _battleModule = battleModule;
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
