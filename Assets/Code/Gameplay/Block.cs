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
        
        private BoxCollider _collider;
        public BoxCollider Collider => _collider;

        
        public void Setup(BlockType blockType, float height)
        {
            BlockType = blockType;
            Height = height;
            

            Health = CalculateHealth(blockType);
            Debug.Log($"New block spawned. Block type {blockType}. Health: {Health}");
            _meshRenderer = GetComponent<MeshRenderer>();
            _meshRenderer.material = GameConfig.BlockStats[blockType].Material;
            _meshRenderer.shadowCastingMode = ShadowCastingMode.On;

            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            _rigidbody.mass = GameConfig.BlockStats[blockType].MassPerUnit * _rigidbody.transform.localScale.magnitude;
            MeshExploder = gameObject.AddComponent<MeshExploder>();
            _collider = gameObject.GetComponent<BoxCollider>();
            _collider.enabled = true;
        }

        private void OnNewContactPointHandler(ContactPoint contactPoint)
        {
            var velocity = contactPoint.impulse.magnitude;
            Debug.Log($"Block velocity: {velocity}");
            if (contactPoint.otherCollider.TryGetComponent(out Block block))
            {
                OnHitBlock?.Invoke(velocity, this, block);
                return;
            }

            if (contactPoint.otherCollider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
            {
                OnHitBlock?.Invoke(velocity, this, null);
            }
        }

        public void Hit(float damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                MeshExploder.Explode(BlockType);
                BlocksManager.RemoveBlock(this);
            }
        }
        
        private void OnCollisionEnter(Collision other)
        {
            OnNewContactPointHandler(other.GetContact(0));
        }
        
        public float GetMass()
        {
            return _rigidbody.mass;
        }

        private float CalculateHealth(BlockType type)
        {
            float health =Mathf.Max(GameConfig.BlockStats[type].Health * transform.localScale.magnitude, GameConfig.BlockStats[type].Health) ;
            return health;
        }
    }
}
