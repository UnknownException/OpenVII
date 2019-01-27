using Engine;
using System;
using TestingModule;

namespace OpenVII
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var engineMgr = new EngineManager())
            {
                /*          
                           var battleMgr = new BattleModuleManager(engineMgr);
                            battleMgr.Register();

                            var miniGameMgr = new MiniGameModuleManager(engineMgr);
                            miniGameMgr.Register();

                            var menuMgr = new MenuModuleManager(engineMgr);
                            menuMgr.Register();

                            var worldMgr = new WorldModuleManager(engineMgr, menuMgr, battleMgr);
                            worldMgr.Register();

                            var fieldMgr = new FieldModuleManager(engineMgr, menuMgr, battleMgr);
                            fieldMgr.Register();
                */

                if (!engineMgr.CreateWindow(800, 600, "OpenVII"))
                {
                    Console.WriteLine("Failed to initialize OpenVII window");
                }

                var testingModule = new TestingModuleManager(engineMgr);
                testingModule.Register();

                engineMgr.Run();
            }
        }
    }
}
