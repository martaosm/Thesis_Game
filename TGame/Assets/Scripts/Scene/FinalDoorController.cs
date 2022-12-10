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
        
        //player prefs if the door was opened already
        private void OnEnable()
        {
            _doorOpened = PlayerPrefs.GetInt("FinalDoorOpened") == 1;
        }

        private void Start()
        {
            //if the door was opened then when player comes back to it from different scene door is still open
            _animator = GetComponent<Animator>();
            if (_doorOpened)
            {
                _animator.SetBool(DoorOpened, true);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            //instruction text is activated when player approaches door
            if (col.TryGetComponent(out PlayerInfo playerInfo))
            {
                instructionsText.gameObject.SetActive(true);
            }
            if (!playerInfo.HasKey)
            {
                instructionsText.text = "You need a key to open it";
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerInfo playerInfo))
            {
                //if player has key then they can open final door
                if (playerInfo.HasKey && !_doorOpened)
                {
                    //if player presses E then animation of opening door is played
                    instructionsText.text = "Press E to open the door";
                    if (Input.GetKey(KeyCode.E))
                    {
                        _animator.SetBool(DoorOpened, true);
                        _doorOpened = true;
                        PlayerPrefs.SetInt("FinalDoorOpened", 1);
                    }
                }

                if (_doorOpened)
                {
                    //if door is open and player presses E, game ends
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
            //if player walks away from door instruction text is deactivated
            if (other.TryGetComponent(out PlayerInfo playerInfo))
            {
                if (playerInfo.HasKey)
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
