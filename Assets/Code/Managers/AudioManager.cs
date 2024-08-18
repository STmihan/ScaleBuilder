using System;
using System.Collections.Generic;
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
            MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
            SoundVolume = PlayerPrefs.GetFloat("SoundVolume", 0.5f);
            StartMusic();
        }
        
        public void PlaySoundOneShot(SoundType soundType)
        {
            _soundSource.PlayOneShot(SoundClips[soundType]);
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