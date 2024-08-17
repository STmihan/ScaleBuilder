using UnityEngine;

namespace Code.Gameplay
{
    public class PlayArea : MonoBehaviour
    {
        [SerializeField] private KillArea _killAreaPrefab;

        private void Start()
        {
            var boxCollider = GetComponent<BoxCollider>();
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    if (x == 0 && y == 0) continue;
                    Instantiate(
                        _killAreaPrefab,
                        new Vector3(x * boxCollider.size.x, transform.position.y, y * boxCollider.size.z),
                        Quaternion.identity,
                        transform
                    );
                }
            }
        }
    }
}