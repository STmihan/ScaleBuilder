using Code.Configs;
using UnityEngine;

namespace Code
{
    public class Block : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;
        
        public BlockType BlockType { get; private set; }
        public float Health { get; private set; }
        public float Height { get; private set; }
        
        private Rigidbody _rigidbody;
        
        public void Setup(BlockType blockType, float height)
        {
            BlockType = blockType;
            Height = height;
            

            Health = GameConfig.Instance.BlockStats[blockType].Health;
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshRenderer.material = GameConfig.Instance.BlockStats[blockType].Material;

            _rigidbody = gameObject.AddComponent<Rigidbody>();
        }
    }
}
