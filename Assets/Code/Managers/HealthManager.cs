using Code.Configs;
using Code.Gameplay;
using Code.Utils;

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
            if (!block2)
            {
                block1.Hit(velocity * GameConfig.TerrainDamageMultiplier);
                return;
            }
            
            block1.Hit(velocity * GameConfig.BlockStats[block2.BlockType].DamageMultiplier);
            block2.Hit(velocity * GameConfig.BlockStats[block1.BlockType].DamageMultiplier);
        }
    }
}