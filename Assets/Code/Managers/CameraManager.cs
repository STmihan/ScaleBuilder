using System;
using System.Collections;
using Code.Utils;
using DG.Tweening;
using Plugins.webgl;
using UnityEngine;

namespace Code.Managers
{
    public class CameraManager : Singleton<CameraManager>
    {
        [SerializeField] private float _rotateSense = 15;
        private float _targetHeight;

        private Camera _camera;
        
        private Quaternion _targetRotation;

        private void Start()
        {
            _camera = Camera.main;
            _targetRotation = transform.rotation;
        }

        private void Update()
        {
            Vector3 pos = Vector3.Slerp(
                transform.position,
                new Vector3(transform.position.x, _targetHeight, transform.position.z),
                Time.deltaTime * 5
            );
            
            transform.position = pos;
            if (Input.GetMouseButton(Platform.IsMobile() ? 1 : 2))
            {
                float dx = Input.mousePositionDelta.x;
                transform.Rotate(Vector3.up, dx * Time.deltaTime * _rotateSense);
                _targetRotation = transform.rotation;
            }
      
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _targetRotation = Quaternion.Euler(0, 90, 0);
            }
            
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.deltaTime * 5);
        }

        public void SetTargetHeight(float height)
        {
            _targetHeight = height;
        }

        public void CameraShake(float duration, float magnitude)
        {
            _camera.DOShakePosition(duration, magnitude);
        }
    }
}