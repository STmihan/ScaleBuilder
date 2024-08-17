using System;
using UnityEngine;

namespace Code.Configs
{
    [Serializable]
    public class BlockStats
    {
        [field: SerializeField] public BlockType BlockType { get; set; }
        [field: SerializeField] public Material Material { get; set; }
        [field: SerializeField] public float Health { get; set; }
    }
}