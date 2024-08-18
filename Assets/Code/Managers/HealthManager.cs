using System;
using Code.Configs;
using Code.Gameplay;
using Code.Utils;
using UnityEngine;

namespace Code.Managers
{
    public class HealthManager : Singleton<HealthManager>
    {
        private GameConfig GameConfig => GameConfig.Instance;
        
        private void Start()
        {
            Block.OnHitBlock += OnHitBlock;
        }

        private void OnHitBlock(float velocity, Block block1, Block block2)
        {
            float damage = 0;
            if (!block2)
            {
                damage = velocity * GameConfig.TerrainDamageMultiplier * block1.GetMass();
                block1.Hit(velocity * GameConfig.TerrainDamageMultiplier * block1.GetMass());
                Debug.Log("Terrain hit. Damage: "+ damage);
                return;
            }
            damage = velocity * GameConfig.BlockStats[block2.BlockType].DamageMultiplier * block2.GetMass();
            block1.Hit(damage);
            Debug.Log("Block 1 hit. Damage: " + damage);
            damage = velocity * GameConfig.BlockStats[block1.BlockType].DamageMultiplier * block1.GetMass();
            Debug.Log("Block 2 hit. Damage: " + damage);
            block2.Hit(damage);
        }
    }
}