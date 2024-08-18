using System;
using Code.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class PauseUI : UIMenu<PauseUI>
    {
        private SettingsUI SettingsUI => SettingsUI.Instance;
        private HelpUI HelpUI => HelpUI.Instance;

        [SerializeField] private Button _startButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _helpButton;

        private void Start()
        {
            _startButton.onClick.AddListener(OnStartButtonClicked);
            _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            _helpButton.onClick.AddListener(OnHelpButtonClicked);
            Hide(true);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (PauseManager.IsPaused && IsVisible)
                {
                    OnStartButtonClicked();
                }
                else if (!PauseManager.IsPaused && !IsVisible && !LevelManager.Instance.IsGameOver)
                {
                    PauseManager.Pause();
                    Show(this);
                }
            }
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
            PauseManager.Resume();
        }
    }
}