using Colourblind.Data;
using Colourblind.Managers;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Colourblind.Systems
{
    public class PlayerUI : MonoBehaviour
    {
        public static PlayerUI Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            SceneTransitionIn();
            pauseMenu.SetActive(false);

            isPaused = false;
        }

        [SerializeField] private Animator sceneTransition;
        [SerializeField] private GameObject sceneTransitionObjectHolder;
        [SerializeField] private Text loadingText;

        [SerializeField] private Animator deathScreenAnimator;

        public Slider loadingProgressSlider;
        public Text loadingProgressText;

        [SerializeField, TextArea] private string loadingMessage;

        [SerializeField] private GameObject pauseMenu;

        public bool isPaused = false;

        private void Start()
        {
            GameManager.Instance.SaveData();

            settingsPanel.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
            {
                if (!isPaused)
                    PauseGame();
                else
                    ResumeGame();
            }

            if (settingsPanel.activeSelf)
            {
                GameManager.Instance.currentSettingsData.gfx = gfxSlider.value / 20;
                GameManager.Instance.currentSettingsData.sens = sensSlider.value / 10;
                GameManager.Instance.currentSettingsData.masterVol = masterVolSlider.value / 10;
                GameManager.Instance.currentSettingsData.musicVol = musicVolSlider.value / 10;
            }
        }

        public void DeathScreen()
        {
            deathScreenAnimator.gameObject.SetActive(true);
            deathScreenAnimator.SetTrigger("Death");

            PlayableDirector[] directors = FindObjectsOfType<PlayableDirector>();
            for (int i = 0; i < directors.Length; i++)
            {
                if (directors[i].playableGraph.IsValid() && directors[i].gameObject.activeSelf)
                    directors[i].playableGraph.GetRootPlayable(0).SetSpeed(0);
            }

            AudioSource[] _pausedAudioSources = FindObjectsOfType<AudioSource>();
            for (int i = 0; i < _pausedAudioSources.Length; i++)
            {
                _pausedAudioSources[i].Pause();
            }

            MusicManager.singleton.PauseTrack();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            StartCoroutine(DeathCoroutine());
        }

        private IEnumerator DeathCoroutine()
        {
            AudioManager.PlaySoundEffect("Kapow");
            yield return new WaitForSeconds(1f);

            Time.timeScale = 0f;

            yield return new WaitForSecondsRealtime(.5f);
            AudioManager.PlaySoundEffect("Sheesh");

            yield return new WaitForSecondsRealtime(.5f);
            AudioManager.PlaySoundEffect("Beep");
        }

        public void SceneTransitionIn()
        {
            sceneTransitionObjectHolder.gameObject.SetActive(false);
            sceneTransition.SetBool("Fade In", true);
            Invoke(nameof(ResetTransition), 1f);
        }

        private void ResetTransition()
        {
            sceneTransition.SetBool("Fade In", false);
        }

        public void SceneTransitionOut()
        {
            sceneTransitionObjectHolder.gameObject.SetActive(true);
            sceneTransition.SetBool("Fade Out", true);
            StartCoroutine(TypeLoadingText());
        }

        private IEnumerator TypeLoadingText()
        {
            yield return new WaitForSeconds(2f);
            for (int i = 0; i < loadingMessage.Length; i++)
            {
                loadingText.text += loadingMessage[i];
                yield return new WaitForSeconds(UnityEngine.Random.Range(.0023f, .0045f));
            }
            yield return new WaitForSeconds(.5f);
            GameManager.Instance.LoadAsyncNextScene();
        }

        private AudioSource[] pausedAudioSources;

        public void PauseGame()
        {
            pauseMenu.SetActive(true);

            settingsPanel.SetActive(false);

            isPaused = true;

            PlayableDirector[] directors = FindObjectsOfType<PlayableDirector>();
            for (int i = 0; i < directors.Length; i++)
            {
                if (directors[i].playableGraph.IsValid() && directors[i].gameObject.activeSelf)
                    directors[i].playableGraph.GetRootPlayable(0).SetSpeed(0);
            }

            pausedAudioSources = FindObjectsOfType<AudioSource>();
            for (int i = 0; i < pausedAudioSources.Length; i++)
            {
                pausedAudioSources[i].Pause();
            }

            MusicManager.singleton.PauseTrack();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0f;
        }

        public void ResumeGame()
        {
            pauseMenu.SetActive(false);

            isPaused = false;

            PlayableDirector[] directors = FindObjectsOfType<PlayableDirector>();
            for (int i = 0; i < directors.Length; i++)
            {
                if (directors[i].playableGraph.IsValid() && directors[i].gameObject.activeSelf)
                    directors[i].playableGraph.GetRootPlayable(0).SetSpeed(1);
            }

            for (int i = 0; i < pausedAudioSources.Length; i++)
            {
                if (pausedAudioSources[i] != null && !pausedAudioSources[i].isPlaying)
                    pausedAudioSources[i].UnPause();
            }

            MusicManager.singleton.ResumeTrack();

            pausedAudioSources = new AudioSource[0];

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Time.timeScale = 1f;
        }

        public void Retry()
        {
            GameManager.Instance.SaveSettings();

            Time.timeScale = 1f;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private Slider gfxSlider, sensSlider, masterVolSlider, musicVolSlider;

        public void Settings()
        {
            SettingsData _settingsData = GameManager.Instance.currentSettingsData;

            gfxSlider.value = _settingsData.gfx * 20f;
            sensSlider.value = _settingsData.sens * 10f;
            masterVolSlider.value = _settingsData.masterVol * 10f;
            musicVolSlider.value = _settingsData.musicVol * 10f;

            settingsPanel.SetActive(true);
        }

        public void MainMenu()
        {
            GameManager.Instance.SaveData();
            GameManager.Instance.SaveSettings();
            SceneManager.LoadScene("Menu");

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 1f;
        }
    }
}