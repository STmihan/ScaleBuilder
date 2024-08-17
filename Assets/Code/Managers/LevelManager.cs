using System;
using Code.Utils;
using UnityEngine;

namespace Code.Managers
{
    public class LevelManager : Singleton<LevelManager>, IRestart
    {
        private BlocksManager BlocksManager => BlocksManager.Instance;
        private MeshManager MeshManager => MeshManager.Instance;
        
        public bool IsGameOver { get; private set; }
        public event Action OnGameOver;
        public int Score => Mathf.RoundToInt(BlocksManager.GetBlocksHeight());

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Restart();
            }
        }
        
        public void GameOver()
        {
            if (IsGameOver) return;
            IsGameOver = true;
            OnGameOver?.Invoke();
        }

        public void Restart()
        {
            IsGameOver = false;
        }
    }
}