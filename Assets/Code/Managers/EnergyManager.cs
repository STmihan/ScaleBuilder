﻿using System;
using Code.Configs;
using Code.Gameplay;
using Code.Utils;
using UnityEngine;

namespace Code.Managers
{
    public class EnergyManager : Singleton<EnergyManager>, IRestart
    {
        public event Action<int> OnFoundationEnergyChanged;
        public event Action<int> OnHeightEnergyChanged;

        private int _foundationEnergy;
        private int _heightEnergy;

        public GameConfig GameConfig => GameConfig.Instance;

        public int FoundationEnergy
        {
            get => _foundationEnergy;
            set
            {
                _foundationEnergy = value;
                OnFoundationEnergyChanged?.Invoke(_foundationEnergy);
            }
        }

        public int HeightEnergy
        {
            get => _heightEnergy;
            set
            {
                _heightEnergy = value;
                OnHeightEnergyChanged?.Invoke(_heightEnergy);
            }
        }

        private void Start()
        {
            FoundationEnergy = GameConfig.MaxFoundationEnergy;
            HeightEnergy = GameConfig.MaxHeightEnergy;
            Block.OnHitBlock += OnHitBlock;
        }

        public int CalculateFoundationEnergyLoss(Vector3 firstPoint, Vector3 secondPoint)
        {
            var distance = Vector3.Distance(firstPoint, secondPoint);
            return Mathf.RoundToInt(distance * GameConfig.FoundationEnergyLossPerUnit);
        }

        public int CalculateHeightEnergyLoss(float height)
        {
            return Mathf.RoundToInt(height * GameConfig.HeightEnergyLossPerUnit);
        }

        private void OnHitBlock(float velocity, Block arg2, Block arg3)
        {
            if (arg3 != null)
            {
                if (velocity > GameConfig.TrashHold)
                {
                    FoundationEnergy += GameConfig.FoundationEnergyGainPerHit;
                    FoundationEnergy = Mathf.Min(FoundationEnergy, GameConfig.MaxFoundationEnergy);
                    HeightEnergy += GameConfig.HeightEnergyGainPerHit;
                    HeightEnergy = Mathf.Min(HeightEnergy, GameConfig.MaxHeightEnergy);
                }
            }
        }

        public void Restart()
        {
            FoundationEnergy = GameConfig.MaxFoundationEnergy;
            HeightEnergy = GameConfig.MaxHeightEnergy;
        }
    }
}