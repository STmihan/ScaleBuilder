using System.Collections;
using UnityEngine;

namespace Code.Gameplay
{
    public class HeightPlane : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;

        private void Start()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void Enable(float height)
        {
            SetHeight(height);
            StartCoroutine(OpacityTo(0.5f));
        }
        
        public void Disable()
        {
            StartCoroutine(OpacityTo(0));
        }
        
        public void SetHeight(float height)
        {
            transform.position = new Vector3(transform.position.x, height + 0.01f, transform.position.z);
        }

        private IEnumerator OpacityTo(float value)
        {
            float currentOpacity = _meshRenderer.material.color.a;
            float targetOpacity = value;
            float duration = 0.5f;
            float elapsedTime = 0;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float newOpacity = Mathf.Lerp(currentOpacity, targetOpacity, elapsedTime / duration);
                _meshRenderer.material.color = new Color(_meshRenderer.material.color.r, _meshRenderer.material.color.g,
                    _meshRenderer.material.color.b, newOpacity);
                yield return null;
            }
        }
    }
}