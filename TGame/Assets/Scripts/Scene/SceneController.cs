using System;
using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UIElements;
using UnityEngine.UI;

namespace Scene
{
    public class SceneController : MonoBehaviour
    {
        private bool _nextChamber;
        private float _dirX;
        private CameraController _cameraController;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject checkpoint;
        [SerializeField] private GameObject pauseCanvas;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button startOverButton;
        [SerializeField] private Button quitButton;
        
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
            if (_nextChamber)
            {
                player.GetComponent<Rigidbody2D>().velocity =
                    new Vector2(_dirX * 10, player.GetComponent<Rigidbody2D>().velocity.y);
            }
            
            if(Input.GetKeyDown(KeyCode.O))
            {
                Time.timeScale = 0;
                pauseCanvas.SetActive(true);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.GetComponent<PlayerInfo>())
            {
                player.GetComponent<PlayerInfo>().InputEnabled = false;
                if (Input.GetAxisRaw("Horizontal")>0)
                {
                    _dirX = 1;
                }else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    _dirX = -1;
                }
                //_dirX = Input.GetAxisRaw("Horizontal");
                _cameraController.enabled = false;
                _nextChamber = true;
                StartCoroutine(MoveToNextChamber());
            }
        }

        IEnumerator MoveToNextChamber()
        {
            yield return new WaitForSeconds(3f);
            if (_dirX > 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }else if (_dirX < 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
        }

        private void ResumeGame()
        {
            pauseCanvas.SetActive(false);
            Time.timeScale = 1;
        }

        private void StarOver()
        {
            pauseCanvas.SetActive(false);
            Time.timeScale = 1;
            SceneManager.LoadScene("MenuScene");
        }

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
