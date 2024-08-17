using Code.Managers;
using Code.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.UI
{
    public class GameOverUI : Singleton<GameOverUI>
    {
        private LevelManager LevelManager => LevelManager.Instance;
        
        [SerializeField] private Image _background;
        [SerializeField] private RectTransform _panel;
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private TMP_Text _scoreText;

        private float _panelY;
        
        private void Start()
        {
            _panelY = _panel.anchoredPosition.y;
            LevelManager.Instance.OnGameOver += Show;
            Hide(true);
            _menuButton.onClick.AddListener(OnMenuButtonClicked);
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
        }

        private void OnRestartButtonClicked()
        {
            RestartManager.Restart();
            Hide();
        }

        private void OnMenuButtonClicked()
        {
            SceneManager.LoadScene(0); // TODO: Load menu scene
        }

        private void Hide(bool instant = false)
        {
            _background.DOFade(0, instant ? 0 : 0.5f);
            _panel.DOAnchorPosY(900, instant ? 0 : 0.5f);
        }

        private void Show()
        {
            _scoreText.text = LevelManager.Score.ToString();
            _background.DOFade(0.8f, 0.5f);
            _panel.DOAnchorPosY(_panelY, 0.5f);
        }
    }
}
