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
            PlayerPrefs.SetInt("IsFree", 0);
            PlayerPrefs.SetInt("GuardEncounters", 0);
            PlayerPrefs.SetInt("DemonGuardDone", 0);
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
