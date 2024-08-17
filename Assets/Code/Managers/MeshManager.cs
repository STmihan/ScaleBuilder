﻿using System;
using System.Collections;
using Code.Configs;
using Code.Gameplay;
using Code.Utils;
using UnityEngine;

namespace Code.Managers
{
    public class MeshManager : Singleton<MeshManager>
    {
        [SerializeField] private float _startHeight = 0.1f;
        [SerializeField] private float _maxHeightPerBlock = 4f;
        [SerializeField] private float _startSubmitTime = 1f;
        
        private CameraManager CameraManager => CameraManager.Instance;
        private BlocksManager BlocksManager => BlocksManager.Instance;
        private LevelManager LevelManager => LevelManager.Instance;
        
        [SerializeField] private HeightPlane _heightPlane;
        
        private BlockType _currentBlockType;
        
        private Vector3 _startPoint;
        private Vector3 _endPoint;
        private GenerationStep _currentStep = GenerationStep.FirstPoint;
        private GameObject _foundation;
        private float _height;
        private float _planeHeight;
        
        private Camera _mainCamera;
        
        private bool _isPlacingBlock;
        
        private Coroutine _submitCoroutine;
        private float _submitTime;

        private void Start()
        {
            _mainCamera = Camera.main;
            _planeHeight = _startHeight;
            CameraManager.SetTargetHeight(_planeHeight);
            _heightPlane.Enable(_planeHeight);
            LevelManager.OnGameOver += OnGameOver;
            _currentBlockType = RandomBlockType();
            _submitTime = _startSubmitTime;
        }

        private void OnGameOver()
        {
            if (_submitCoroutine != null) StopCoroutine(_submitCoroutine);
        }

        private void Update()
        {
            if (_isPlacingBlock) return;
            if (LevelManager.IsGameOver) return;
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, new Vector3(0, _planeHeight, 0));

            if (plane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                switch (_currentStep)
                {
                    case GenerationStep.FirstPoint:
                        ProcessFirstPoint(hitPoint);
                        break;

                    case GenerationStep.Foundation:
                        ProcessFoundation(hitPoint);
                        break;

                    case GenerationStep.Height:
                        ProcessHeight();
                        break;
                }
            }
        }
        
        private void ProcessFirstPoint(Vector3 hitPoint)
        {
            _startPoint = hitPoint;
            if (Input.GetMouseButtonDown(0))
            {
                _foundation = GameObject.CreatePrimitive(PrimitiveType.Cube);
                _foundation.transform.position = _startPoint;
                _foundation.transform.localScale =
                    new Vector3(_startHeight, _startHeight, _startHeight);
                _currentStep = GenerationStep.Foundation;
                _height = _startHeight;
            }
        }
        
        private void ProcessFoundation(Vector3 hitPoint)
        {
            _endPoint = hitPoint;

            Vector3 baseSize = new Vector3(Mathf.Abs(_endPoint.x - _startPoint.x), _startHeight,
                Mathf.Abs(_endPoint.z - _startPoint.z));
            _foundation.transform.localScale = baseSize;

            _foundation.transform.position = _startPoint + new Vector3((_endPoint.x - _startPoint.x) / 2, 0,
                (_endPoint.z - _startPoint.z) / 2);

            if (Input.GetMouseButtonDown(0))
            {
                _currentStep = GenerationStep.Height;
            }
        }
        
        private void ProcessHeight()
        {
            _height += Input.mousePositionDelta.y * 0.03f;
                        
            _height = Mathf.Clamp(_height, _startHeight, _maxHeightPerBlock);
            _foundation.transform.localScale = new Vector3(_foundation.transform.localScale.x, _height,
                _foundation.transform.localScale.z);
            _foundation.transform.position = new Vector3(_foundation.transform.position.x,
                _planeHeight + _height / 2, _foundation.transform.position.z);
                        
            if (Input.GetMouseButtonDown(0))
            {
                _submitCoroutine = StartCoroutine(Submit());
            }
        }

        private IEnumerator Submit()
        {
            _currentStep = GenerationStep.FirstPoint;
            Block block = _foundation.AddComponent<Block>();
            block.Setup(_currentBlockType, _height);
            BlocksManager.AddBlock(block);
            _heightPlane.Disable();
            _isPlacingBlock = true;
            while (_submitTime > 0)
            {
                if (BlocksManager.IsAnyBlockMoving()) _submitTime = _startSubmitTime;
                _submitTime -= Time.deltaTime;
                yield return null;
            }
            _submitTime = _startSubmitTime;
            _currentBlockType = RandomBlockType();
            _planeHeight = BlocksManager.GetBlocksHeight();
            CameraManager.SetTargetHeight(_planeHeight);
            _heightPlane.Enable(_planeHeight);
            _isPlacingBlock = false;
        }

        public void Restart()
        {
            if (_submitCoroutine != null) StopCoroutine(_submitCoroutine);
            if (_foundation) Destroy(_foundation);
            _isPlacingBlock = false;
            _currentStep = GenerationStep.FirstPoint;
            _planeHeight = _startHeight;
            _heightPlane.SetHeight(_startHeight);
            _heightPlane.Enable(_planeHeight);
            CameraManager.SetTargetHeight(_planeHeight);
        }
        
        private BlockType RandomBlockType()
        {
            var values = Enum.GetValues(typeof(BlockType));
            return (BlockType) values.GetValue(UnityEngine.Random.Range(0, values.Length));
        }
    }
}