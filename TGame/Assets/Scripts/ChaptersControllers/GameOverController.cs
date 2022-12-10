using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ChaptersControllers
{
    /**
     * Class managing game over screen
     */
    public class GameOverController : MonoBehaviour
    {
        [SerializeField] private Button playAgainButton;
        [SerializeField] private Button quitButton;

        
        //adding listeners to buttons
        private void OnEnable()
        {
            playAgainButton.onClick.AddListener(PlayAgain);
            quitButton.onClick.AddListener(QuitGame);
        }

        //if button clicked then game is reloaded
        private void PlayAgain()
        {
            SceneManager.LoadScene("MenuScene");
        }
    
        //if button clicked the player quits game
        private void QuitGame()
        {
            Application.Quit();
        }

        //removing all listeners 
        private void OnDisable()
        {
            playAgainButton.onClick.RemoveAllListeners();
            quitButton.onClick.RemoveAllListeners();
        }
    }
}
