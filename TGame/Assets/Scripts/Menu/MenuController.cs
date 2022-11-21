using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button quitButton;

        private void OnEnable()
        {
            startButton.onClick.AddListener(LoadScene);
            quitButton.onClick.AddListener(QuitGame);
        }

        private void LoadScene()
        {
            SceneManager.LoadScene("Prologue");
            PlayerPrefs.SetInt("FireNpcEncounters", 0);
            PlayerPrefs.SetInt("hasKey", 0);
            PlayerPrefs.SetInt("hasMark", 0);
            PlayerPrefs.SetInt("PlayerHealth", 100);
            PlayerPrefs.SetInt("IsFree", 0); //is npc in chapter1 let put by player
            PlayerPrefs.SetInt("GuardEncounters", 0);//how many times player trigger conversation with guard
            PlayerPrefs.SetInt("DemonGuardDone", 0);//player dealt with guard
            PlayerPrefs.SetInt("CandyGiverDealt", 0);
        }

        private void QuitGame()
        {
            Application.Quit();
        }

        private void OnDisable()
        {
            startButton.onClick.RemoveAllListeners();
            quitButton.onClick.RemoveAllListeners();
        }
    }
}
