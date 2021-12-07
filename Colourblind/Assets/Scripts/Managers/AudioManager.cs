using Colourblind.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Colourblind.Managers
{
    public class AudioManager : MonoBehaviour
    {
        #region Singleton
        public static AudioManager Instance;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        #endregion
        public List<SoundEffect> SoundEffects = new List<SoundEffect>();

        private AudioSource source;

        private void Start()
        {
            source = GetComponent<AudioSource>();
        }

        public static void PlaySoundEffect(string ID, float _vol = -1)
        {
            AudioSource _source = Instance.source;
            SoundEffect _sfx = GetSoundEffect(ID);
            if (_sfx.clip != null)
            {
                if (_vol == -1)
                    _source.PlayOneShot(_sfx.clip, _sfx.volume);
                else
                    _source.PlayOneShot(_sfx.clip, _vol);
            }
            else
            {
                Debug.LogError("Audio clip not found");
            }
        }

        public static void PlayProfiledSoundEffect(string ID, Transform t, float vol = -99f, float spatialBlend = 0f, float maxSpatialDist = 17f, float pitch = 1f)
        {
            SoundEffect _sfx = GetSoundEffect(ID);
            if (_sfx.clip != null)
            {
                GameObject _sfxSourceObj = new GameObject();
                _sfxSourceObj.name = _sfx.ID;
                _sfxSourceObj.transform.position = t.position;

                AudioSource _source = _sfxSourceObj.AddComponent<AudioSource>();
                _source.clip = _sfx.clip;

                //Volume Setting
                if (vol == -99f)
                {
                    _source.volume = _sfx.volume;
                }
                else
                {
                    _source.volume = vol;
                }

                //Other Settings
                _source.spatialBlend = spatialBlend;
                _source.maxDistance = maxSpatialDist;
                _source.pitch = pitch;

                _source.Play();

                Destroy(_source.gameObject, _sfx.clip.length);
            }
            else
            {
                Debug.LogError("Audio clip not found");
            }
        }

        public static SoundEffect GetSoundEffect(string ID)
        {
            SoundEffect sfx = new SoundEffect();
            List<SoundEffect> SoundEffectList = Instance.SoundEffects;
            for (int i = 0; i < SoundEffectList.Count; i++)
            {
                if (SoundEffectList[i].ID == ID)
                {
                    sfx = SoundEffectList[i];
                }
            }
            return sfx;
        }
    }
}