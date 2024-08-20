using Code.Configs;
using Code.Managers;
using Code.Utils;
using Plugins.webgl;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class HintsUI : Singleton<HintsUI>
    {
        private MeshManager MeshManager => MeshManager.Instance;

        [SerializeField] private GameObject _normalHint;
        [SerializeField] private GameObject _mobileHint;
        [Header("Mobile")] [SerializeField] private GameObject _placeBlockHint;
        [SerializeField] private GameObject _selectBlockHeightHint;
        [SerializeField] private Button _pauseButton;

        private LevelManager LevelManager => LevelManager.Instance;

        private CanvasGroup _canvasGroup;

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _pauseButton.onClick.AddListener(PauseManager.Pause);
        }

        private void Update()
        {
            _normalHint.SetActive(!Platform.IsMobile());
            _mobileHint.SetActive(Platform.IsMobile());
            _canvasGroup.alpha = LevelManager.IsGameOver ? 0 : 1;
            if (Platform.IsMobile())
            {
                if (MeshManager.Processing)
                {
                    _canvasGroup.alpha = 0;
                }
                if (MeshManager.CurrentStep == GenerationStep.FirstPoint)
                {
                    _placeBlockHint.SetActive(true);
                    _selectBlockHeightHint.SetActive(false);
                }
                else
                {
                    _placeBlockHint.SetActive(false);
                    _selectBlockHeightHint.SetActive(true);
                }
            }
        }
    }
}