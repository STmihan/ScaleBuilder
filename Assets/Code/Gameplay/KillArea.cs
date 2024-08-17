using Code.Managers;
using UnityEngine;

namespace Code.Gameplay
{
    public class KillArea : MonoBehaviour
    {
        private LevelManager LevelManager => LevelManager.Instance;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Block block))
            {
                LevelManager.GameOver();
            }
        }
    }
}