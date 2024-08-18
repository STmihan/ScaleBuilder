using Code.Utils;
using UnityEngine;

namespace Code.Gameplay.Explosions
{
    public class DebugMeshExploder : MonoBehaviour
    {
        [SerializeField] private Block _block;

        private void Start()
        {
            _block.Setup(BlockType.Wood, 1f);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _block.Hit(20f);
            }
        }
    }
}
