using System.Collections.Generic;
using Code.UI;
using Code.Utils;

namespace Code.Managers
{
    public static class RestartManager
    {
        private static readonly List<IRestart> Restartables = new()
        {
            BlocksManager.Instance,
            LevelManager.Instance,
            MeshManager.Instance,
            InGameUI.Instance,
            EnergyManager.Instance,
        };
        
        public static void Restart()
        {
            foreach (var restartable in Restartables)
            {
                restartable.Restart();
            }
        }
    }
}
