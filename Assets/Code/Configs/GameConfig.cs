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
        [field: SerializeField] public int MaxFoundationEnergy { get; private set; } = 100;
        [field: SerializeField] public int FoundationEnergyLossPerUnit { get; private set; } = 4;
        [field: SerializeField] public int FoundationEnergyGainPerRound { get; private set; } = 5;
        [field: SerializeField] public int FoundationEnergyGainPerHit { get; private set; } = 5;
        [field: SerializeField] public int FoundationEnergyGainVelocityMultiplier { get; private set; } = 5;
        [field: SerializeField] public int MaxHeightEnergy { get; private set; } = 100;
        [field: SerializeField] public int HeightEnergyLossPerUnit { get; private set; } = 1;
        [field: SerializeField] public int HeightEnergyGainPerRound { get; private set; } = 5;
        [field: SerializeField] public int HeightEnergyGainPerHit { get; private set; } = 10;
        [field: SerializeField] public int HeightEnergyGainVelocityMultiplier { get; private set; } = 1;

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