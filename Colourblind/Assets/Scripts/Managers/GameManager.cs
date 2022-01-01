using Colourblind.Data;
using Colourblind.Enums;
using Colourblind.Systems;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Colourblind.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private GameObject[] heldCubes, interactableCubes;

        public delegate void OnLoadScene();

        public static OnLoadScene onLoadScene;

        public SaveData currentSaveData;
        public SettingsData currentSettingsData;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            } else
            {
                Destroy(this.gameObject);
                return;
            }

            onLoadScene += LoadNextScene;
            SceneManager.sceneLoaded += OnSceneFinishedLoaded;
        }

        private void Update()
        {
            AudioListener.volume = currentSettingsData.masterVol / 2f;
            if (MusicManager.singleton != null)
            {
                if (MusicManager.singleton.currentTrack != null && MusicManager.singleton.currentTrack._audioClip != null)
                {
                    MusicManager.singleton.musicSource.volume = MusicManager.singleton.currentTrack.volume * currentSettingsData.musicVol * 2f;
                }
            }
        }

        public void OnSceneFinishedLoaded(Scene scene, LoadSceneMode mode)
        {
            if (SceneManager.GetActiveScene().name != "Menu")
                SaveSettings();
        }

        public static GameObject GetHeldCube(CoreColour _colour)
        {
            int i = ((int)_colour) - 1;

            if (i >= 0)
                return Instance.heldCubes[i];
            else
                return null;
        }

        public static GameObject GetInteractableCube(CoreColour _colour)
        {
            int i = ((int)_colour) - 1;

            if (i >= 0)
                return Instance.interactableCubes[i];
            else
                return null;
        }
        
        public static CoreColour GetCombinedCube(CoreColour _colour1, CoreColour _colour2)
        {
            CoreColour returnColour = CoreColour.None;

            if (_colour1 == CoreColour.Red)
            {
                if (_colour2 == CoreColour.Blue)
                {
                    //Red + Blue = Purple
                    returnColour = CoreColour.Purple;
                } else if (_colour2 == CoreColour.Yellow)
                {
                    //Red + Yellow = Orange
                    returnColour = CoreColour.Orange;
                }
            } 
            
            else if (_colour1 == CoreColour.Blue)
            {
                if (_colour2 == CoreColour.Red)
                {
                    //Blue + Red = Purple
                    returnColour = CoreColour.Purple;
                }
                else if (_colour2 == CoreColour.Yellow)
                {
                    //Blue + Yellow = Green
                    returnColour = CoreColour.Green;
                }
            } 
            
            else if (_colour1 == CoreColour.Yellow)
            {
                if (_colour2 == CoreColour.Red)
                {
                    //Yellow + Red = Orange
                    returnColour = CoreColour.Orange;
                }
                else if (_colour2 == CoreColour.Blue)
                {
                    //Yellow + Blue = Green
                    returnColour = CoreColour.Green;
                }
            }

            return returnColour;
        }

        /// <summary>
        /// Purple = Red + Blue
        /// Orange = Red + Yellow
        /// Green = Blue + Yellow
        /// </summary>
        /// <param name="_colour"></param>
        /// <returns></returns>
        public static CoreColour[] GetDisassembledCubes(CoreColour _colour)
        {
            CoreColour[] returnColourList = new CoreColour[2] { CoreColour.None, CoreColour.None };

            if (_colour == CoreColour.Purple)
            {
                //Purple = Red + Blue
                returnColourList = new CoreColour[2] { CoreColour.Red, CoreColour.Blue };
            } else if (_colour == CoreColour.Orange)
            {
                //Orange = Red + Yellow
                returnColourList = new CoreColour[2] { CoreColour.Red, CoreColour.Yellow };
            } else if (_colour == CoreColour.Green)
            {
                //Green = Blue + Yellow
                returnColourList = new CoreColour[2] { CoreColour.Blue, CoreColour.Yellow };
            }

            return returnColourList;
        }

        public static bool CheckDissasembledCubeIsValid(CoreColour[] _color)
        {
            return (_color[0] != CoreColour.None && _color[1] != CoreColour.None);
        }

        private void LoadNextScene()
        {
            if (Player.Instance != null)
            {
                Player.Instance.DisableMovement(false);
            }

            MusicManager.singleton.StopTrack();

            PlayerUI.Instance.SceneTransitionOut();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            SaveSettings();
        }

        public void LoadAsyncNextScene()
        {
            StartCoroutine(LoadNextSceneAsync());
        }

        private IEnumerator LoadNextSceneAsync()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / .9f);

                if (PlayerUI.Instance != null)
                {
                    PlayerUI.Instance.loadingProgressSlider.value = progress;
                    PlayerUI.Instance.loadingProgressText.text = progress * 100 + "%";
                }

                yield return null;
            }
        }

        public void SaveData()
        {
            if (GetCurrentChapterIndex() != 0)
            {
                if (currentSaveData == null)
                {
                    currentSaveData = new SaveData();
                }

                currentSaveData.currentScene = SceneManager.GetActiveScene().name;
                currentSaveData.saveDate = DateTime.UtcNow.ToLocalTime().ToString("dd-MM-yyyy HH:mm");

                Debug.Log(GetStartingChapterScene(GetCurrentChapterIndex()));

                SaveManager.SaveData(currentSaveData, GetChapterSave(GetCurrentChapterIndex()), "saveData");
            }
        }

        public static int GetCurrentChapterIndex()
        {
            if (SceneManager.GetActiveScene().name.StartsWith("Factory"))
            {
                return 1;
            } else
            {
                return 0;
            }
        }

        public static string GetStartingChapterScene(int chapterIndex)
        {
            if (chapterIndex == 1)
            {
                return "Factory-1";
            }
            else
            {
                return "Factory-1";
            }
        }

        public static string GetChapterSave(int chapterIndex)
        {
            return "chapter" + chapterIndex;
        }

        public void LoadGame(SaveData saveData)
        {
            currentSaveData = saveData;

            SceneManager.LoadScene(saveData.currentScene);
        }

        public void SaveSettings()
        {
            if (currentSettingsData == null)
            {
                Debug.Log("Current Settings Data null");
                currentSettingsData = new SettingsData();
            }

            Debug.Log("Saving settings data...");

            SaveManager.SaveData(currentSettingsData, "settings", "settingsData");
        }

        public SettingsData LoadSettings()
        {
            if (SaveManager.FileExists("settings", "settingsData"))
            {
                Debug.Log("File exists");
                SettingsData _settings = (SettingsData) SaveManager.Load("settings", "settingsData");
                currentSettingsData = _settings;

                currentSettingsData.gfx = _settings.gfx;
                currentSettingsData.sens = _settings.sens;
                currentSettingsData.masterVol = _settings.masterVol;
                currentSettingsData.musicVol = _settings.musicVol;
            } else
            {
                Debug.Log("File doesn't exist");
                SettingsData _settings = new SettingsData();
                currentSettingsData = _settings;
            }

            return currentSettingsData;
        }

        private void OnApplicationQuit()
        {
            SaveData();
            SaveSettings();
        }
    }
}