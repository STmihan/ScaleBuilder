﻿using System;
using Code.Gameplay;
using UnityEngine;

namespace Code.Configs
{
    [Serializable]
    public class BlockStats
    {
        [field: SerializeField] public BlockType BlockType { get; set; }
        [field: SerializeField] public Material Material { get; set; }
        [field: SerializeField] public float Health { get; set; }
        [field: SerializeField] public float DamageMultiplier { get; set; }
        [field: SerializeField] public float MassPerUnit { get; set; }
    }
}