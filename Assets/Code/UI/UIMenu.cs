using Code.Managers;
using Code.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public abstract class UIMenu<T> : Singleton<T> where T : UIMenu<T>
    {
        private AudioManager AudioManager => AudioManager.Instance;
        
        [SerializeField] private Image _background;
        [SerializeField] private RectTransform _panel;

        private float _panelY;
        private GraphicRaycaster _raycaster;
        public bool IsVisible { get; private set; }
        public MonoBehaviour PreviousMenu { get; private set; }

        private void Awake()
        {
            _panelY = _panel.anchoredPosition.y;
            _raycaster = GetComponent<GraphicRaycaster>();
        }

        public void Hide(bool instant = false, bool hideBackground = true)
        {
            if (instant)
            {
                _background.gameObject.SetActive(false);
                _panel.gameObject.SetActive(false);
                _background.color = new Color(_background.color.r, _background.color.g, _background.color.b, 0);
                _panel.anchoredPosition = new Vector2(_panel.anchoredPosition.x, 900);
                IsVisible = false;
                _raycaster.enabled = false;
            }
            else
            {
                AudioManager.PlaySoundOneShot(SoundType.UIOut);
                var sequence = DOTween.Sequence();
                if (!hideBackground)
                {
                    _background.gameObject.SetActive(false);
                    IsVisible = false;
                    _raycaster.enabled = false;
                }
                else sequence.Join(_background.DOFade(0, 0.5f));

                sequence.Join(_panel.DOAnchorPosY(900, 0.5f));
                sequence.AppendCallback(() => _panel.gameObject.SetActive(false));
                sequence.AppendCallback(() => IsVisible = false);
                sequence.AppendCallback(() => _raycaster.enabled = false);
                sequence.SetUpdate(true);
            }
        }

        public void Show<TMenu>(TMenu previousMenu) where TMenu : UIMenu<TMenu>
        {
            PreviousMenu = previousMenu;
            _background.gameObject.SetActive(true);
            _panel.gameObject.SetActive(true);
            
            AudioManager.PlaySoundOneShot(SoundType.UIIn);
            
            var sequence = DOTween.Sequence();
            sequence.Join(_background.DOFade(0.8f, 0.5f));
            sequence.Join(_panel.DOAnchorPosY(_panelY, 0.5f));
            sequence.AppendCallback(() => IsVisible = true);
            sequence.AppendCallback(() => _raycaster.enabled = true);
            sequence.SetUpdate(true);
        }
    }
}