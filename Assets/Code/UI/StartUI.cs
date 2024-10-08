﻿using Code.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class StartUI : UIMenu<StartUI>
    {
        private SettingsUI SettingsUI => SettingsUI.Instance;
        private HelpUI HelpUI => HelpUI.Instance;
        private LevelManager LevelManager => LevelManager.Instance;

        [SerializeField] private Button _startButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _helpButton;
        [SerializeField] private TMP_Text _highScoreText;

        private void Start()
        {
            _startButton.onClick.AddListener(OnStartButtonClicked);
            _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            _helpButton.onClick.AddListener(OnHelpButtonClicked);
            Hide(true);
            Show(this);
            _highScoreText.text = LevelManager.HighScore.ToString();
            LevelManager.OnGameOver += OnGameOver;
        }

        private void OnGameOver()
        {
            _highScoreText.text = LevelManager.HighScore.ToString();
        }

        private void OnHelpButtonClicked()
        {
            Hide(false, false);
            HelpUI.Show(this);
        }

        private void OnSettingsButtonClicked()
        {
            Hide(false, false);
            SettingsUI.Show(this);
        }

        private void OnStartButtonClicked()
        {
            Hide();
            RestartManager.Restart();
        }
    }
}