using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalDoorController : MonoBehaviour
{
    private bool _doorOpened;
    private Animator _animator;
    [SerializeField] private TextMeshProUGUI instructionsText;
    private static readonly int DoorOpened = Animator.StringToHash("DoorOpened");

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out PlayerInfo playerInfo))
        {
            instructionsText.gameObject.SetActive(true);
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
}
