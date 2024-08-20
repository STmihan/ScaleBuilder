using System;
using Code.Configs;
using Code.Managers;
using Code.Utils;
using Plugins.webgl;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Gameplay
{
    public class CancelArea : MonoBehaviour
    {
        [SerializeField] private Image _image;
        private CanvasGroup _canvasGroup;
        private MeshManager MeshManager => MeshManager.Instance;
        private LevelManager LevelManager => LevelManager.Instance;

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            if (!Platform.IsMobile())
            {
                _canvasGroup.alpha = 0;
                return;
            }
            Vector2 mousePosition = Input.mousePosition;
            float alpha = MeshManager.Processing ? 0.7f : 0f;
            if (RectTransformUtility.RectangleContainsScreenPoint(_image.rectTransform, mousePosition))
            {
                if (Input.GetMouseButtonUp(0))
                {
                    MeshManager.Cancel();
                }
            }
            
            _canvasGroup.alpha = LevelManager.IsGameOver ? 0 : alpha;
        }
    }
}