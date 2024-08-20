using Code.UI;
using UnityEngine;

namespace Code.Managers
{
    public static class PauseManager
    {
        public static bool IsPaused { get; private set; }
        
        public static void Pause()
        {
            IsPaused = true;
            Time.timeScale = 0;
            PauseUI.Instance.Show<PauseUI>(null);
        }
        
        public static void Resume()
        {
            IsPaused = false;
            Time.timeScale = 1;
            PauseUI.Instance.Hide();
        }
    }
}