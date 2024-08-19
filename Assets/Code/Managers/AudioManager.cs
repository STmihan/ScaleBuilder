using System;
using System.Collections.Generic;
using Code.Gameplay;
using Code.Utils;
using UnityEngine;

namespace Code.Managers
{
    public enum SoundType
    {
        UITap,
        UIIn,
        UIOut,
        Hit,
        Explosion,
        GameOver,
    }

    [Serializable]
    public class SoundData
    {
        [field: SerializeField] public SoundType SoundType { get; private set; }
        [field: SerializeField] public AudioClip AudioClip { get; private set; }
    }

    public class AudioManager : Singleton<AudioManager>
    {
        private LevelManager LevelManager => LevelManager.Instance;
        
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _soundSource;
        
        [SerializeField] private List<SoundData> _soundClips;
        [SerializeField] private List<AudioClip> _musicClips;

        private IReadOnlyDictionary<SoundType, AudioClip> SoundClips { get; set; }

        public float MusicVolume
        {
            get => _musicSource.volume;
            set
            {
                PlayerPrefs.SetFloat("MusicVolume", value);
                _musicSource.volume = value;
            }
        }

        public float SoundVolume
        {
            get => _soundSource.volume;
            set
            {
                PlayerPrefs.SetFloat("SoundVolume", value);
                _soundSource.volume = value;
            }
        }
        
        private int _currentMusicClipIndex;
        
        private void Awake()
        {
            SoundClips = SetupSoundClips();
            MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.3f);
            SoundVolume = PlayerPrefs.GetFloat("SoundVolume", 0.3f);
            StartMusic();
            Block.OnHitBlock += OnHitBlock;
            LevelManager.OnGameOver += OnGameOver;
        }

        private void OnGameOver()
        {
            PlaySoundOneShot(SoundType.GameOver);
        }

        private void OnHitBlock(float velocity, Block block1, Block block2)
        {
            float soundVolume = Mathf.Clamp(velocity, 0.1f, 1);
            PlaySoundOneShot(SoundType.Hit, soundVolume);
        }

        public void PlaySoundOneShot(SoundType soundType, float volume = 1)
        {
            if (!SoundClips.TryGetValue(soundType, out var clip))
            {
                Debug.LogError($"Sound type {soundType} not found");
                return;
            }
            
            _soundSource.volume *= volume;
            _soundSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            _soundSource.PlayOneShot(clip);
            _soundSource.pitch = 1;
            _soundSource.volume /= volume;
        }

        private void StartMusic()
        {
            _musicSource.clip = _musicClips[0];
            _currentMusicClipIndex = 0;
            _musicSource.Play();
        }

        private void Update()
        {
            if (!_musicSource.isPlaying)
            {
                _currentMusicClipIndex = (_currentMusicClipIndex + 1) % _musicClips.Count;
                _musicSource.clip = _musicClips[_currentMusicClipIndex];
                _musicSource.Play();
            }
        }

        private IReadOnlyDictionary<SoundType, AudioClip> SetupSoundClips()
        {
            var dictionary = new Dictionary<SoundType, AudioClip>();
            foreach (var soundData in _soundClips)
            {
                dictionary[soundData.SoundType] = soundData.AudioClip;
            }
            return dictionary;
        }
    }
}