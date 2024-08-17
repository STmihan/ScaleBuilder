using System.Collections.Generic;
using Code.Gameplay;
using UnityEngine;

namespace Code.Configs
{
    [CreateAssetMenu(menuName = "Create Game Config", fileName = "GameConfig", order = 0)]
    public class GameConfig : ScriptableObject
    {
        private const string GameConfigPath = "GameConfig";
        private static GameConfig _instance;
        
        public static GameConfig Instance => _instance ??= Setup();

        [SerializeField] private List<BlockStats> _blockMaterialPairs = new();
        
        public IReadOnlyDictionary<BlockType, BlockStats> BlockStats { get; private set; }

        private static GameConfig Setup()
        {
            var config = Resources.Load<GameConfig>(GameConfigPath);
            
            var dictionary = new Dictionary<BlockType, BlockStats>();
            foreach (var blockMaterialPair in config._blockMaterialPairs)
            {
                dictionary[blockMaterialPair.BlockType] = blockMaterialPair;
            }
            config.BlockStats = dictionary;
            
            return config;
        }
    }
}