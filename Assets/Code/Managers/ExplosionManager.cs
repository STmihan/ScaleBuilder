using System.Collections.Generic;
using Code.Configs;
using Code.Gameplay;
using Code.Utils;
using DG.Tweening;
using UnityEngine;

namespace Code.Managers
{
    public class ShatterPart
    {
        public Rigidbody Rigidbody { get; set; }
        public MeshFilter MeshFilter { get; set; }
        public MeshRenderer MeshRenderer { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class ExplosionData
    {
        public Vector3 ExplosionPos { get; set; }
        public float ExplosionForce { get; set; }
        public float ExplosionRadius { get; set; }
        public ShatterPart[] ShatterParts { get; set; }
    }

    public class ExplosionManager : Singleton<ExplosionManager>
    {
        private GameConfig GameConfig => GameConfig.Instance;
        private const int ShatterPartsPoolSize = 690;

        [SerializeField] private List<GameObject> _shatterPrefabs;
        [SerializeField] private int _shatterCountPerUnit = 5;
        [SerializeField] private float _explosionForcePerUnit = 1000;
        [SerializeField] private float _explosionRadiusPerUnit = 1;

        private List<ShatterPart> _shatterParts = new List<ShatterPart>();
        private Queue<ExplosionData> _explosionData = new();

        private Vector3 _shatterScaleCache;

        public void Explode(Vector3 position, Vector3 scale, BlockType type)
        {
            var explosionPos = position;
            var shatterCount = Mathf.RoundToInt(scale.magnitude * _shatterCountPerUnit);

            var shatterParts = new ShatterPart[shatterCount];
            for (int i = 0, j = 0; i < _shatterParts.Count && j < shatterCount; i++)
            {
                var shatterPart = _shatterParts[i];
                if (shatterPart.IsAvailable)
                {
                    shatterParts[j] = shatterPart;
                    shatterPart.IsAvailable = false;
                    shatterPart.MeshRenderer.material = GameConfig.BlockStats[type].Material;
                    j++;
                }
            }

            _explosionData.Enqueue(new ExplosionData
            {
                ExplosionPos = explosionPos,
                ExplosionForce = _explosionForcePerUnit * scale.magnitude,
                ExplosionRadius = _explosionRadiusPerUnit * scale.magnitude,
                ShatterParts = shatterParts
            });
        }

        private void Start()
        {
            DOTween.SetTweensCapacity(ShatterPartsPoolSize * 2, ShatterPartsPoolSize * 2);
            for (int i = 0; i < ShatterPartsPoolSize; i++)
            {
                var toSpawn = _shatterPrefabs[i % _shatterPrefabs.Count];
                var instance = Instantiate(toSpawn, Vector3.zero, Quaternion.identity, transform);
                instance.SetActive(false);
                instance.GetComponent<MeshCollider>();
                var shatterPart = new ShatterPart
                {
                    Rigidbody = instance.GetComponent<Rigidbody>(),
                    MeshFilter = instance.GetComponent<MeshFilter>(),
                    MeshRenderer = instance.GetComponent<MeshRenderer>(),
                    IsAvailable = true
                };
                _shatterScaleCache = shatterPart.MeshFilter.transform.localScale;

                _shatterParts.Add(shatterPart);
            }
        }

        private void Update()
        {
            if (_explosionData.Count > 0)
            {
                ProcessExplosion(_explosionData.Dequeue());
            }
        }

        private void ProcessExplosion(ExplosionData data)
        {
            foreach (var shatterPart in data.ShatterParts)
            {
                shatterPart.MeshFilter.transform.position = data.ExplosionPos + Random.insideUnitSphere;
                shatterPart.MeshFilter.gameObject.SetActive(true);
                shatterPart.Rigidbody.AddExplosionForce(data.ExplosionForce, data.ExplosionPos, data.ExplosionRadius);
                ReturnShatterPartsToPool(shatterPart);
            }

            RaycastHit[] hits = new RaycastHit[100];
            Physics.SphereCastNonAlloc(data.ExplosionPos, data.ExplosionRadius, Vector3.up, hits);
            foreach (var hit in hits)
            {
                if (!hit.collider) continue;
                if (hit.collider.TryGetComponent(out Rigidbody rb))
                {
                    rb.AddExplosionForce(data.ExplosionForce, data.ExplosionPos, data.ExplosionRadius);
                }
            }
        }

        private void ReturnShatterPartsToPool(ShatterPart shatterPart, bool instant = false)
        {
            if (instant)
            {
                shatterPart.MeshFilter.gameObject.SetActive(false);
                shatterPart.IsAvailable = true;
                return;
            }

            DOTween.Sequence()
                .AppendInterval(5)
                .Append(shatterPart.Rigidbody.transform.DOScale(Vector3.zero, 0.5f))
                .AppendCallback(() =>
                {
                    shatterPart.MeshFilter.gameObject.SetActive(false);
                    shatterPart.IsAvailable = true;
                    shatterPart.Rigidbody.transform.localScale = _shatterScaleCache;
                });
        }
    }
}