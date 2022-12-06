using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private GameObject controlsPanel;
        [SerializeField] private Button startButton;
        [SerializeField] private Button controlsButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button backButton;

        private void OnEnable()
        {
            startButton.onClick.AddListener(LoadScene);
            controlsButton.onClick.AddListener(ShowControls);
            quitButton.onClick.AddListener(QuitGame);
            backButton.onClick.AddListener(BackToStart);
        }

        private void LoadScene()
        {
            SceneManager.LoadScene("Prologue");
            PlayerPrefs.SetInt("FireNpcEncounters", 0);
            PlayerPrefs.SetInt("hasKey", 0);
            PlayerPrefs.SetInt("hasMark", 0);
            PlayerPrefs.SetInt("PlayerHealth", 100);
            PlayerPrefs.SetInt("IsFree", 0); //is npc in chapter1 let free by player
            PlayerPrefs.SetInt("GuardEncounters", 0);//how many times player trigger conversation with guard
            PlayerPrefs.SetInt("DemonGuardDone", 0);//player dealt with guard
            PlayerPrefs.SetInt("CandyGiverDealt", 0);
            PlayerPrefs.SetInt("FinalDoorOpened", 0);
            PlayerPrefs.SetString("spawnPoint", "left");
        }

        private void ShowControls()
        {
            controlsPanel.SetActive(true);
        }

        private void QuitGame()
        {
            Application.Quit();
        }

        private void BackToStart()
        {
            controlsPanel.SetActive(false);
        }
        
            private void OnDisable()
        {
            startButton.onClick.RemoveAllListeners();
            controlsButton.onClick.RemoveAllListeners();
            quitButton.onClick.RemoveAllListeners();
            backButton.onClick.RemoveAllListeners();
        }
    }
}
