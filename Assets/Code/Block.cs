using UnityEngine;

namespace Code
{
    public enum BlockType
    {
        Wood,
        Stone,
        Metal,
    }
    
    public class Block : MonoBehaviour
    {
        public BlockType BlockType { get; set; }
    }
}
