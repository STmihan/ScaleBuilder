﻿using System;
using System.Collections;
using Code.Utils;
using DG.Tweening;
using UnityEngine;

namespace Code.Managers
{
    public class CameraManager : Singleton<CameraManager>
    {
        [SerializeField] private float _rotateSense = 15;
        private float _targetHeight;
        
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            transform.position = Vector3.Slerp(transform.position,
                new Vector3(transform.position.x, _targetHeight, transform.position.z),
                Time.deltaTime * 5);
            if (Input.GetMouseButton(2))
            {
                Cursor.lockState = CursorLockMode.Locked;
                float dx = Input.mousePositionDelta.x;
                transform.Rotate(Vector3.up, dx * Time.deltaTime * _rotateSense);
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
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