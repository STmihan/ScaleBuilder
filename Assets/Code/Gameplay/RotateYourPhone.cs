using Code.Utils;
using Plugins.webgl;
using UnityEngine;

namespace Code.Gameplay
{
    public class RotateYourPhone : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private bool Active()
        {
            if (Platform.IsMobile())
            {
                return Screen.width <= Screen.height;
            }

            return false;
        }

        private void Update()
        {
            _canvasGroup.alpha = Active() ? 1 : 0;
            _canvasGroup.interactable = Active();
            _canvasGroup.blocksRaycasts = Active();
        }
    }
}