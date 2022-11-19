using Player;
using TMPro;
using UnityEngine;

public class FinalDoorController : MonoBehaviour
{
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
            if (playerInfo._hasKey)
            {
              instructionsText.gameObject.SetActive(true);
              instructionsText.text = "Press E to open the door";  
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerInfo playerInfo))
        {
            if (playerInfo._hasKey)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    _animator.SetBool(DoorOpened, true);
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
