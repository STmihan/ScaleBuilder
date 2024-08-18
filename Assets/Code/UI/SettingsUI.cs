using Code.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class SettingsUI : UIMenu<SettingsUI>
    {
        private StartUI StartUI => StartUI.Instance;
        private PauseUI PauseUI => PauseUI.Instance;
        private AudioManager AudioManager => AudioManager.Instance;
        
        [SerializeField] private Button _backButton;
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _soundSlider;
        
        private void Start()
        {
            _backButton.onClick.AddListener(OnBackButtonClicked);
            _musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
            _soundSlider.onValueChanged.AddListener(OnSoundSliderValueChanged);
            Hide(true);
            _soundSlider.value = AudioManager.SoundVolume;
            _musicSlider.value = AudioManager.MusicVolume;
        }

        private void OnSoundSliderValueChanged(float arg0)
        {
            AudioManager.SoundVolume = arg0;
        }

        private void OnMusicSliderValueChanged(float arg0)
        {
            AudioManager.MusicVolume = arg0;
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