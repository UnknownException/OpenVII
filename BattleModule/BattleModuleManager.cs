using Shared.Engine;
using Shared.Modules;
using System;

namespace BattleModule
{
    public class BattleModuleManager : IBattleModuleManager
    {
        private readonly IEngineManager _engineManager;

        public BattleModuleManager(IEngineManager engineManager)
        {
            _engineManager = engineManager;            
        }

        public void OnDraw()
        {
            throw new NotImplementedException();
        }

        public void OnUpdate(long deltaTime)
        {
            throw new NotImplementedException();
        }

        public void Register()
        {
            throw new NotImplementedException();
        }
    }
}
