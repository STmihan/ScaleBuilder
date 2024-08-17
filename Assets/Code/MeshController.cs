using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code
{
    [Serializable]
    public class BlockMaterialPair
    {
        [field: SerializeField] public BlockType BlockType { get; set; }
        [field: SerializeField] public Material Material { get; set; }
    }

    public enum GenerationStep
    {
        FirstPoint,
        Foundation,
        Height,
    }

    public class MeshController : MonoBehaviour
    {
        [FormerlySerializedAs("_cameraBeamPoint")] [SerializeField] private CameraBeam _cameraBeam;
        private Vector3 _startPoint;
        private Vector3 _endPoint;
        private GenerationStep _currentStep = GenerationStep.FirstPoint;
        private GameObject _foundation;
        private float _height;
        private float _planeHeight;
        
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
            _planeHeight = 0.1f;
            _cameraBeam.SetTargetHeight(_planeHeight);
        }

        private void Update()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, new Vector3(0, _planeHeight, 0));

            if (plane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);
                switch (_currentStep)
                {
                    case GenerationStep.FirstPoint:
                        _startPoint = hitPoint;
                        if (Input.GetMouseButtonDown(0))
                        {
                            _foundation = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            _foundation.transform.position = _startPoint;
                            _foundation.transform.localScale =
                                new Vector3(0.1f, 0.1f, 0.1f);
                            _currentStep = GenerationStep.Foundation;
                            _height = 0.1f;
                        }

                        break;

                    case GenerationStep.Foundation:
                        _endPoint = hitPoint;

                        Vector3 baseSize = new Vector3(Mathf.Abs(_endPoint.x - _startPoint.x), 0.1f,
                            Mathf.Abs(_endPoint.z - _startPoint.z));
                        _foundation.transform.localScale = baseSize;

                        _foundation.transform.position = _startPoint + new Vector3((_endPoint.x - _startPoint.x) / 2, 0,
                            (_endPoint.z - _startPoint.z) / 2);

                        if (Input.GetMouseButtonDown(0))
                        {
                            _currentStep = GenerationStep.Height;
                        }

                        break;

                    case GenerationStep.Height:
                        _height += Input.mousePositionDelta.y * 0.03f;
                        
                        if (_height < 0.1f)
                        {
                            _height = 0.1f;
                        }
                        _foundation.transform.localScale = new Vector3(_foundation.transform.localScale.x, _height,
                            _foundation.transform.localScale.z);
                        _foundation.transform.position = new Vector3(_foundation.transform.position.x,
                            _planeHeight + _height / 2, _foundation.transform.position.z);
                        
                        if (Input.GetMouseButtonDown(0))
                        {
                            _planeHeight += _height;
                            _cameraBeam.SetTargetHeight(_planeHeight);
                            _currentStep = GenerationStep.FirstPoint;
                        }

                        break;
                }
            }
        }
    }
}