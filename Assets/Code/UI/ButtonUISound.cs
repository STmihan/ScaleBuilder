using Code.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class ButtonUISound : MonoBehaviour
    {
        private AudioManager AudioManager => AudioManager.Instance;
        
        private Button _button;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(PlaySound);
        }

        private void PlaySound() => AudioManager.PlaySoundOneShot(SoundType.UITap);
    }
}