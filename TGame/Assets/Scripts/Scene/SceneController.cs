using System.Collections;
using Camera;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scene
{
    public class SceneController : MonoBehaviour
    {
        private bool _nextChamber;
        private float _dirX;
        private CameraController _cameraController;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject pauseCanvas;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button startOverButton;
        [SerializeField] private Button quitButton;

        //adds listeners to buttons
        private void OnEnable()
        {
            resumeButton.onClick.AddListener(ResumeGame);
            startOverButton.onClick.AddListener(StarOver);
            quitButton.onClick.AddListener(QuitGame);
        }
    
        private void Start()
        {
            _cameraController = FindObjectOfType<CameraController>();
        }

        private void Update()
        {
            //if player wants to move to next chamber, then they have to step into trigger near the exit, when player touches a trigger
            //they start running without input in the direction of the exit 
            if (_nextChamber)
            {
                player.GetComponent<Rigidbody2D>().velocity =
                    new Vector2(_dirX * 10, player.GetComponent<Rigidbody2D>().velocity.y);
            }
            
            //when player presses O pause screen is activated
            if(Input.GetKeyDown(KeyCode.O))
            {
                Time.timeScale = 0;
                pauseCanvas.SetActive(true);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            //when player enters trigger camera stops moving along with the player and coroutine starts
            if (col.gameObject.GetComponent<PlayerInfo>())
            {
                player.GetComponent<PlayerInfo>().InputEnabled = false;
                if (Input.GetAxisRaw("Horizontal")>0)
                {
                    _dirX = 1;
                }
                else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    _dirX = -1;
                }
                _cameraController.enabled = false;
                _nextChamber = true;
                gameObject.SetActive(true);
                StartCoroutine(MoveToNextChamber());
            }
        }

        //depending on direction in which player runs, scene changes to the next or previous one
        private IEnumerator MoveToNextChamber()
        {
            yield return new WaitForSeconds(3f);
            if (_dirX > 0)
            {
                PlayerPrefs.SetString("spawnPoint", "left");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else if (_dirX < 0)
            {
                PlayerPrefs.SetString("spawnPoint", "right");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
        }

        //after pressing resume button game continues
        private void ResumeGame()
        {
            pauseCanvas.SetActive(false);
            Time.timeScale = 1;
        }

        //after pressing start over button game is reloaded
        private void StarOver()
        {
            pauseCanvas.SetActive(false);
            Time.timeScale = 1;
            SceneManager.LoadScene("MenuScene");
        }

        //after pressing quit button game is turned off
        private void QuitGame()
        {
            pauseCanvas.SetActive(false);
            Application.Quit();
        }

        private void OnDisable()
        {
            resumeButton.onClick.RemoveAllListeners();
            startOverButton.onClick.RemoveAllListeners();
            quitButton.onClick.RemoveAllListeners();
        }
    }
}
