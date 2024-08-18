using Code.Managers;
using Code.Utils;
using UnityEngine;

namespace Code.UI
{
    public class HintsUI : Singleton<HintsUI>
    {
        private LevelManager LevelManager => LevelManager.Instance;
        
        private CanvasGroup _canvasGroup;
        
        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        
        private void Update()
        {
            _canvasGroup.alpha = LevelManager.IsGameOver ? 0 : 1;
        }
    }
}