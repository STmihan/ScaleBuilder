using UnityEngine;

namespace Plugins.webgl
{
    public class DebugWebGL : MonoBehaviour
    {
        public static DebugWebGL Instance { get; private set; }
        [SerializeField] private bool _isMobile;
        public bool IsMobile => _isMobile;
        
        private void Awake()
        {
            Instance = this;
        }
    }
}