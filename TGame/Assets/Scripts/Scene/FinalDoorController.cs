using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene
{
    public class FinalDoorController : MonoBehaviour
    {
        private bool _doorOpened;
        private Animator _animator;
        [SerializeField] private TextMeshProUGUI instructionsText;
        private static readonly int DoorOpened = Animator.StringToHash("DoorOpened");


        private void OnEnable()
        {
            _doorOpened = PlayerPrefs.GetInt("FinalDoorOpened") == 1;
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
            if (_doorOpened)
            {
                _animator.SetBool(DoorOpened, true);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent(out PlayerInfo playerInfo))
            {
                instructionsText.gameObject.SetActive(true);
            }
            if (!playerInfo._hasKey)
            {
                instructionsText.text = "You need a key to open it";
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerInfo playerInfo))
            {
                if (playerInfo._hasKey && !_doorOpened)
                {
                    instructionsText.text = "Press E to open the door";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        _animator.SetBool(DoorOpened, true);
                        _doorOpened = true;
                        PlayerPrefs.SetInt("FinalDoorOpened", 1);
                    }
                }

                if (_doorOpened)
                {
                    instructionsText.text = "Press E to pass through";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        SceneManager.LoadScene("Ending");
                    }
                }
            
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerInfo playerInfo))
            {
                if (playerInfo._hasKey)
                {
                    instructionsText.gameObject.SetActive(false);
                }
            }
        }

        private void OnDisable()
        {
            PlayerPrefs.SetInt("FinalDoorOpened", _doorOpened ? 1 : 0);
        }
    }
}
