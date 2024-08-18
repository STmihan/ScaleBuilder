using System;
using Code.Utils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public abstract class UIMenu<T> : Singleton<T> where T : UIMenu<T>
    {
        [SerializeField] private Image _background;
        [SerializeField] private RectTransform _panel;

        private float _panelY;

        private void Awake()
        {
            _panelY = _panel.anchoredPosition.y;
        }

        public void Hide(bool instant = false, bool hideBackground = true)
        {
            if (instant)
            {
                _background.gameObject.SetActive(false);
                _panel.gameObject.SetActive(false);
                _background.color = new Color(_background.color.r, _background.color.g, _background.color.b, 0);
                _panel.anchoredPosition = new Vector2(_panel.anchoredPosition.x, 900);
            }
            else
            {
                if (hideBackground)
                    _background.DOFade(0, 0.5f).OnComplete(() => _background.gameObject.SetActive(false));
                else _background.gameObject.SetActive(false);
                _panel.DOAnchorPosY(900, 0.5f).OnComplete(() => _panel.gameObject.SetActive(false));
            }
        }

        public void Show()
        {
            _background.gameObject.SetActive(true);
            _panel.gameObject.SetActive(true);
            _background.DOFade(0.8f, 0.5f);
            _panel.DOAnchorPosY(_panelY, 0.5f);
        }
    }
}