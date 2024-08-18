using Code.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class StartUI : UIMenu<StartUI>
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
            Show();
        }

        private void OnHelpButtonClicked()
        {
            Hide(false, false);
            HelpUI.Show();
        }

        private void OnSettingsButtonClicked()
        {
            Hide(false, false);
            SettingsUI.Show();
        }

        private void OnStartButtonClicked()
        {
            Hide();
            RestartManager.Restart();
        }
    }
}