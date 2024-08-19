using Code.Managers;
using UnityEngine;

namespace Code.Gameplay.Explosions
{
    public class MeshExploder : MonoBehaviour
    {
        private AudioManager AudioManager => AudioManager.Instance;
        private ExplosionManager ExplosionManager => ExplosionManager.Instance;

        public void Explode(BlockType type)
        {
            AudioManager.PlaySoundOneShot(SoundType.Explosion);
            var explosionPos = transform.position;
            var scale = transform.localScale;
            ExplosionManager.Explode(explosionPos, scale, type);
        }
    }
}