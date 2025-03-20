using UnityEngine;
using UnityEngine.Audio;

namespace BombDrop.Global
{
    public enum AudioTypes { CLICK_BUTTON, DROP_BOMB, BOMB_EXPLOSION, MISSILE_EXPLOSION, AA_VEHICLE_SHOOT, PLAYER_SHOOT }
    public enum AudioMixers { SFX, BGM, ENGINE }

    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClip[] _audioClips;
        [SerializeField] private AudioSource _sfxAudioSource, _bgmAudioSource, _engineAudioSource;
        [SerializeField] private AudioMixerGroup _sfxAudioMixer, _bgmAudioMixer;

        #region Singleton
        private static AudioManager _instance;
        public static AudioManager Instance { get => _instance; }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
        }
        #endregion Singleton

        public void PlaySFXClip(AudioTypes audioClipType, float volumeScale = 1f)
        {
            _sfxAudioSource.PlayOneShot(_audioClips[(int)audioClipType], volumeScale);
        }

        public void PauseAll(bool pauseStatus)
        {
            if (pauseStatus)
            {
                _engineAudioSource.Pause();
                // _bgmAudioSource.Pause();
            }
            else
            {
                _engineAudioSource.UnPause();
                // _bgmAudioSource.UnPause();
            }
        }

        public void PlaySFXClip(int audioClipIndex)
        {
            _sfxAudioSource.PlayOneShot(_audioClips[audioClipIndex]);
        }

        public void SetAudioSourcesLevels(AudioMixers audioMixer, float value)
        {
            switch (audioMixer)
            {
                case AudioMixers.SFX:
                    _sfxAudioMixer.audioMixer.SetFloat("SFXVolume", value);
                    break;

                case AudioMixers.BGM:
                    _bgmAudioMixer.audioMixer.SetFloat("BGMVolume", value);
                    break;

                case AudioMixers.ENGINE:
                    _engineAudioSource.volume = value;
                    break;
            }
        }
    }
}
