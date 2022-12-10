using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

namespace Menu
{
    /**
     * Class controls menu scene
     */
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private GameObject controlsPanel;
        [SerializeField] private Button startButton;
        [SerializeField] private Button controlsButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button backButton;
        
        //adds listeners to all buttons
        private void OnEnable()
        {
            startButton.onClick.AddListener(LoadScene);
            controlsButton.onClick.AddListener(ShowControls);
            quitButton.onClick.AddListener(QuitGame);
            backButton.onClick.AddListener(BackToStart);
        }

        //when scene is loaded all player prefs are set to default value
        private void LoadScene()
        {
            SceneManager.LoadScene("Prologue"); //scene load
            PlayerPrefs.SetInt("FireNpcEncounters", 0); //how many times player encountered npc in cell, this value helps with controlling dialog options
            PlayerPrefs.SetInt("hasKey", 0); //if player have key in their possession, 0 - no , 1 - yes
            PlayerPrefs.SetInt("hasMark", 0); //if player have mark in their possession, 0 - no , 1 - yes
            PlayerPrefs.SetInt("PlayerHealth", 100); //player's health points
            PlayerPrefs.SetInt("IsFree", 0); //is npc in chapter1 let free by player
            PlayerPrefs.SetInt("GuardEncounters", 0); //how many times player trigger conversation with guard
            PlayerPrefs.SetInt("DemonGuardDone", 0); //if player finished interaction with a guard
            PlayerPrefs.SetInt("CandyGiverDealt", 0); //if player finished interaction with this npc
            PlayerPrefs.SetInt("FinalDoorOpened", 0); //if player opened final door, if opened animation stays the same 
            PlayerPrefs.SetString("spawnPoint", "left"); //on which spawn points player is appearing
            GenerateKeyPosition();
        }

        //generates in which scene key will appear
        private void GenerateKeyPosition()
        {
            var random = new Random();
            PlayerPrefs.SetInt("keyPosition", random.Next(3));
        }
        
        //activates panel with control instructions displayed
        private void ShowControls()
        {
            controlsPanel.SetActive(true);
        }

        //application quit
        private void QuitGame()
        {
            Application.Quit();
        }

        //turns off control instruction panel
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
