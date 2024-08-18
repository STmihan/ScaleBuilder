using System;
using Code.Utils;
using UnityEngine;

namespace Code.Managers
{
    public class LevelManager : Singleton<LevelManager>, IRestart
    {
        private BlocksManager BlocksManager => BlocksManager.Instance;

        public bool IsGameOver { get; private set; } = true;
        public event Action OnGameOver;
        public int Score => Mathf.RoundToInt(BlocksManager.GetBlocksHeight()*100);
        public int HighScore { get; private set; }

        private void Awake()
        {
            HighScore = PlayerPrefs.GetInt("HighScore", 0);
        }

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
            HighScore = Mathf.Max(HighScore, Score);
            PlayerPrefs.SetInt("HighScore", HighScore);
            OnGameOver?.Invoke();
        }

        public void Restart()
        {
            IsGameOver = false;
        }
    }
}