using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class HelpUI : UIMenu<HelpUI>
    {
        private StartUI StartUI => StartUI.Instance;
        
        [SerializeField] private Button _backButton;
        
        private void Start()
        {
            _backButton.onClick.AddListener(OnBackButtonClicked);
            Hide(true);
        }

        private void OnBackButtonClicked()
        {
            Hide(false, false);
            StartUI.Show();
        }
    }
}