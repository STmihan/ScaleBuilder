using System;
using Code.Configs;
using Code.Gameplay.Explosions;
using Code.Managers;
using UnityEngine;
using UnityEngine.Rendering;

namespace Code.Gameplay
{
    public class Block : MonoBehaviour
    {
        public static event Action<float, Block, Block> OnHitBlock; 
        private GameConfig GameConfig => GameConfig.Instance;
        private BlocksManager BlocksManager => BlocksManager.Instance;
        
        private MeshRenderer _meshRenderer;
        
        public BlockType BlockType { get; private set; }
        public float Health { get; private set; }
        public float Height { get; private set; }
        public MeshExploder MeshExploder { get; private set; }
        
        public Vector3 Velocity => _rigidbody.velocity;
        
        private Rigidbody _rigidbody;
        
        public void Setup(BlockType blockType, float height)
        {
            BlockType = blockType;
            Height = height;
            

            Health = GameConfig.BlockStats[blockType].Health;
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshRenderer.material = GameConfig.BlockStats[blockType].Material;
            _meshRenderer.shadowCastingMode = ShadowCastingMode.On;

            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.mass = GameConfig.BlockStats[blockType].Mass;
            MeshExploder = gameObject.AddComponent<MeshExploder>();
        }

        public void Hit(float damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                MeshExploder.Explode();
                BlocksManager.RemoveBlock(this);
            }
        }
        
        private void OnCollisionEnter(Collision other)
        { 
            Debug.Log(other.relativeVelocity);
            if (other.gameObject.GetComponent<Terrain>())
            {
                Debug.Log("Block hit terrain");
                OnHitBlock?.Invoke(Height, this, null);
            }
            if (other.gameObject.TryGetComponent<Block>(out var block))
            {
                Debug.Log("Block hit block " + block.BlockType);
                OnHitBlock?.Invoke(Height, this, block);
            }
        }
    }
}
