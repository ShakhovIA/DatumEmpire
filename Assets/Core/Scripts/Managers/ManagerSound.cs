using System.Collections;
using System.Collections.Generic;
using Injection;
using UnityEngine;

namespace Core.Scripts
{
    public sealed class ManagerSound : MonoBehaviour
    {

        #region [Injections]

        [Inject] private ManagerAccount ManagerAccount { get; set; }

        #endregion
        
        
        #region [Fields]
        
        [field: SerializeField] private AudioChannel MainChannel { get; set; }
        [field: SerializeField] private List<AudioChannel> Channels { get; set; }
        [field: SerializeField] public AudioClip AudioMainMenu { get; private set; }
        [field: SerializeField] public AudioClip AudioWin { get; private set; }
        [field: SerializeField] public AudioClip AudioLose { get; private set; }
        [field: SerializeField] public AudioClip AudioButtonClick { get; private set; }
        
        #endregion

        
        #region [Functions]
        private void Awake()
        {
            if (AudioMainMenu)
                PlayTheme(AudioMainMenu);
            //LoadSettings();
        }

        public void SetMusicVolume(float _value)
        {
            MainChannel.source.volume = _value / 5;
        }

        public void MuteMusic()
        {
            MainChannel.source.Stop();
        }
        
        public void UnMuteMusic()
        {
            MainChannel.source.Play();
        }
        

        public void SetSoundEffectsVolume(float _value)
        {
            foreach (var chanel in Channels)
                chanel.source.volume = _value;
        }

        public void SetSoundEffectsVolume(UnityEngine.UI.Slider _slider)
        {
            foreach (var chanel in Channels)
                chanel.volume = _slider.value;

            //SaveSettings();
        }

        public void MuteAllSounds()
        {
            foreach (var chanel in Channels)
                chanel.source.mute = true;

            MainChannel.source.mute = true;
        }

        public void UnMuteAllSounds()
        {
            foreach (var chanel in Channels)
                chanel.source.mute = false;

            MainChannel.source.mute = false;
        }

        public void PlayTheme(AudioClip _clip)
        {
            MainChannel.source.clip = _clip;
            MainChannel.source.loop = true;
            MainChannel.source.Play();
            MainChannel.source.volume = 0.2f;
            //mainChannel.source.volume = DataAccount.DataVolume;
        }


        [System.Obsolete]
        public void PlayEffect(AudioClip _clip, int _channel, float _volume)
        {
            Channels[_channel].source.loop = false;
            Channels[_channel].SetNewClipStart(_clip, _volume);
        }
        
        public void PlayEffect(AudioClip _clip)
        {
            foreach (var channel in Channels)
            {
                if (channel.source.isPlaying is false)
                {
                    channel.source.loop = false;
                    channel.SetNewClipStart(_clip);
                    break;
                }
            }
        }


        public void StopEffect(int _channel)
        {
            Channels[_channel].source.Stop();
        }

        public void StopAllEffects()
        {
            foreach (var channel in Channels)
            {
                channel.source.Stop();
            }
        }

        /// <summary>
        /// Корутина отложенного старта проигрывания аудио
        /// </summary>
        /// <param name="_delay">Задержка до начала проигрывания</param>
        /// <param name="_source">Дорожка</param>
        /// <returns></returns>
        public IEnumerator DelayedStart(float _delay, AudioSource _source)
        {
            yield return new WaitForSeconds(_delay);

            _source.Stop();
            _source.Play();
        }

        private void SaveSettings()
        {
            // string _musicVolumeSettings = string.Empty;
            //
            // _musicVolumeSettings += mainChannel.source.volume.ToString() + ",";
            //
            // foreach (var chanel in channels)
            //     _musicVolumeSettings += $"{chanel.source.volume},";

            PlayerPrefs.SetFloat("musicVolume", MainChannel.volume);
            PlayerPrefs.SetFloat("effectVolume", Channels[0].volume);
            PlayerPrefs.Save();
        }

        private void LoadSettings()
        {
            if (PlayerPrefs.GetFloat("musicVolume") > 0)
            {
                SetMusicVolume(PlayerPrefs.GetFloat("musicVolume"));
                SetSoundEffectsVolume(PlayerPrefs.GetFloat("effectVolume"));
            }

        }
        
        #endregion

    }

    [System.Serializable]
    public class AudioChannel
    {
        public AudioSource source;
        public AudioClip clip;

        [Range(0f, 1f)] public float volume = 0.3f;
        [Range(.1f, 3f)] public float pitch = 0.5f;

        public void SetNewClipStart(AudioClip value)
        {
            if (!value)
            {
                return;
            }

            clip = value;
            source.clip = clip;
            source.volume = volume;
            source.Stop();
            source.Play();
        }

        [System.Obsolete]
        public void SetNewClipStart(AudioClip value, float vol)
        {
            if (!value)
            {
                return;
            }

            clip = value;
            source.clip = clip;
            source.volume = vol;
            source.Stop();
            source.Play();
        }
    }
}

