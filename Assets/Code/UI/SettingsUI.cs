using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class SettingsUI : UIMenu<SettingsUI>
    {
        private StartUI StartUI => StartUI.Instance;
        private PauseUI PauseUI => PauseUI.Instance;
        
        [SerializeField] private Button _backButton;
        
        private void Start()
        {
            _backButton.onClick.AddListener(OnBackButtonClicked);
            Hide(true);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (IsVisible)
                {
                    OnBackButtonClicked();
                }
            }
        }
        
        private void OnBackButtonClicked()
        {
            Hide(false, false);
            if (PreviousMenu is StartUI)
            {
                StartUI.Show(this);
            }
            else if (PreviousMenu is PauseUI)
            {
                PauseUI.Show(this);
            }
        }
    }
}