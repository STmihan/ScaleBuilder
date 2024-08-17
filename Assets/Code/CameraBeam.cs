using UnityEngine;

namespace Code
{
    public class CameraBeam : MonoBehaviour
    {
        [SerializeField] private float _rotateSense = 15;
        private float _targetHeight;

        private void Update()
        {
            transform.position = Vector3.Slerp(transform.position,
                new Vector3(transform.position.x, _targetHeight, transform.position.z),
                Time.deltaTime * 5);
            if (Input.GetMouseButton(1))
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
    }
}