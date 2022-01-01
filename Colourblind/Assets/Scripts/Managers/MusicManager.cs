using Colourblind.Data;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Colourblind.Managers
{
    public class MusicManager : MonoBehaviour
    {
        #region Singleton
        public static MusicManager singleton;

        private void Awake()
        {
            if (singleton == null)
                singleton = this;
        }
        #endregion

        public AudioSource musicSource;
        public MusicTrack currentTrack;

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void PlayTrack(MusicTrack musicTrack)
        {
            if (musicSource.isPlaying && currentTrack._audioClip != null)
                musicSource.Stop();

            currentTrack = musicTrack;
            currentTrack._queuedLoopClip = musicTrack._queuedLoopClip;

            musicSource.clip = musicTrack._audioClip;
            musicSource.volume = musicTrack.volume;
            musicSource.loop = musicTrack.loop;

            musicSource.Play();
        }

        private void Update()
        {
            if (musicSource.clip != null && currentTrack._audioClip != null && currentTrack._queuedLoopClip != null && !musicSource.isPlaying && musicSource.clip == currentTrack._audioClip)
            {
                musicSource.clip = currentTrack._queuedLoopClip;
                musicSource.loop = true;
                musicSource.Play();
            }
        }

        public void StopTrack()
        {
            if (musicSource.isPlaying && currentTrack._audioClip != null)
            {
                if (currentTrack.fadeOut)
                {
                    StartCoroutine(FadeOutTrack());
                }
                else
                {
                    musicSource.Stop();
                    musicSource.clip = null;
                }

                currentTrack._audioClip = null;
            }
        }

        public void PauseTrack()
        {
            if (musicSource.isPlaying)
            {
                musicSource.Pause();
            }
        }

        public void ResumeTrack()
        {
            if (!musicSource.isPlaying)
            {
                musicSource.UnPause();
            }
        }

        private IEnumerator FadeOutTrack()
        {
            while (musicSource.isPlaying && musicSource.volume != 0f)
            {
                musicSource.volume = Mathf.Lerp(musicSource.volume, 0f, 1.2f * TimeManager.GetFixedDeltaTime());
                yield return null;
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (currentTrack.exitUnload)
            {
                StopTrack();
            }
        }
    }
}