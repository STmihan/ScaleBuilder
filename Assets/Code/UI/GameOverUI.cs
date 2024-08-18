using Code.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class GameOverUI : UIMenu<GameOverUI>
    {
        private LevelManager LevelManager => LevelManager.Instance;
        private StartUI StartUI => StartUI.Instance;
        
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _highScoreText;

        
        private void Start()
        {
            LevelManager.OnGameOver += OnGameOver;
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
            Hide(false, false);
            StartUI.Show(this);
        }

        private void OnGameOver()
        {
            _scoreText.text = LevelManager.Score.ToString();
            _highScoreText.text = LevelManager.HighScore.ToString();
            Show(this);
        }
    }
}
