using Colourblind.Data;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace Colourblind.Managers
{
    public class MenuManager : MonoBehaviour
    {
        public GameObject startGamePanel, extrasPanel, settingsPanel;
        public SaveBlock[] saveBlocks;
        [SerializeField] private GameObject interactSave;
        [SerializeField] private Slider gfxSlider, sensSlider, masterVolSlider, musicVolSlider;
        [SerializeField] private PostProcessVolume postProcessEffects;

        private void Start()
        {
            startGamePanel.SetActive(false);
            extrasPanel.SetActive(false);
            settingsPanel.SetActive(false);

            for (int i = 0; i < saveBlocks.Length; i++)
            {
                saveBlocks[i].arrow.SetActive(false);
            }

            AssignSettings();
        }

        private void Update()
        {
            interactSave.SetActive(currentSaveSelection != -1);

            GameManager.Instance.currentSettingsData.gfx = (float) gfxSlider.value / 20;
            GameManager.Instance.currentSettingsData.sens = (float) sensSlider.value / 10;
            GameManager.Instance.currentSettingsData.masterVol = (float) masterVolSlider.value / 10;
            GameManager.Instance.currentSettingsData.musicVol = (float) musicVolSlider.value / 10;

            postProcessEffects.weight = Mathf.Clamp(GameManager.Instance.currentSettingsData.gfx, 0.5f, 1f);
        }

        private void AssignSettings()
        {
            SettingsData _settingsData = GameManager.Instance.LoadSettings();

            gfxSlider.value = _settingsData.gfx * 20f;
            sensSlider.value = _settingsData.sens * 10f;
            masterVolSlider.value = _settingsData.masterVol * 10f;
            musicVolSlider.value = _settingsData.musicVol * 10f;
        }

        private void UpdateSaves()
        {
            for (int i = 0; i < saveBlocks.Length; i++)
            {
                if (SaveManager.FileExists(GameManager.GetChapterSave(i + 1), "saveData"))
                {
                    saveBlocks[i].FillSaveDate(
                        ((SaveData) SaveManager.Load(GameManager.GetChapterSave(i + 1), "saveData")).saveDate
                    );
                } else
                {
                    saveBlocks[i].FillSaveDate("EMPTY SAVE");
                }
            }
        }

        public void ActivatePanel(int panelIndex)
        {
            switch (panelIndex)
            {
                case 0:
                    startGamePanel.SetActive(true);
                    extrasPanel.SetActive(false);
                    settingsPanel.SetActive(false);
                    UpdateSaves();
                    break;
                case 1:
                    startGamePanel.SetActive(false);
                    extrasPanel.SetActive(true);
                    settingsPanel.SetActive(false);
                    break;
                case 2:
                    startGamePanel.SetActive(false);
                    extrasPanel.SetActive(false);
                    settingsPanel.SetActive(true);
                    break;
            }
        }

        private int currentSaveSelection = -1;

        public void SelectSave(int selectionIndex)
        {
            for (int i = 0; i < saveBlocks.Length; i++)
            {
                saveBlocks[i].arrow.SetActive(false);
            }

            saveBlocks[selectionIndex].arrow.SetActive(true);

            currentSaveSelection = selectionIndex + 1;
        }

        public void ExitGame()
        {
            GameManager.Instance.SaveSettings();
            Debug.Log("Exitting Game...");
            Application.Quit();
        }

        public void LoadSave()
        {
            if (currentSaveSelection == 1)
            {
                if (SaveManager.FileExists(GameManager.GetChapterSave(currentSaveSelection), "saveData"))
                {
                    GameManager.Instance.LoadGame((SaveData) SaveManager.Load(GameManager.GetChapterSave(currentSaveSelection), "saveData"));
                } else
                {
                    GameManager.Instance.LoadGame(new SaveData() { currentScene = GameManager.GetStartingChapterScene(currentSaveSelection) });
                }
            }
        }

        public void DeleteSave()
        {
            if (currentSaveSelection == 1)
            {
                if (SaveManager.FileExists(GameManager.GetChapterSave(currentSaveSelection), "saveData"))
                {
                    SaveManager.Delete(GameManager.GetChapterSave(currentSaveSelection), "saveData");
                }

                UpdateSaves();
            }
        }
    }
}