using System;
using UnityEngine;

namespace Code.Gameplay.Explosions
{
    public class DebugMeshExploder : MonoBehaviour
    {
        [SerializeField] private MeshExploder _meshExploder;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _meshExploder.Explode();
            }
        }
    }
}