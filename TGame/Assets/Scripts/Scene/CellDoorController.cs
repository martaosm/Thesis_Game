using System.Collections;
using NPC;
using Player;
using TMPro;
using UnityEngine;

namespace Scene
{
    public class CellDoorController : MonoBehaviour
    {
        private bool _usedKey;
        private PlayerInfo _playerInfo;
        private FireNpcController _fireNpc;
        private bool _doorOpened;
        public bool DoorOpened
        {
            get => _doorOpened;
            set => _doorOpened = value;
        }
        [SerializeField] private GameObject cellDoor;
        [SerializeField] private TextMeshProUGUI instructionText;
        [SerializeField] private GameObject panel;
        private static readonly int CellDoorOpened = Animator.StringToHash("CellDoorOpened");


        private void OnEnable()
        {
            _playerInfo = FindObjectOfType<PlayerInfo>();
            _fireNpc = FindObjectOfType<FireNpcController>();
        }

        private void Update()
        {
            //if player uses key, cell door are opened 
            if (_usedKey)
            {
                cellDoor.GetComponent<Animator>().SetBool(CellDoorOpened, true);
                cellDoor.GetComponent<Collider2D>().enabled = false;
                _usedKey = false;
                _doorOpened = true;
                _playerInfo.InputEnabled = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            //text with information is displayed
            if (_playerInfo.HasKey)
            {
                instructionText.text = "Press E to use Key";
                instructionText.gameObject.SetActive(true);
            }
            panel.SetActive(true);
            _fireNpc.EncountersCount++;
            _fireNpc.ConvoController();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            //if player is in trigger and presses then noc is let free and attacks player
            if (other.gameObject.GetComponent<PlayerInfo>() 
                && Input.GetKey(KeyCode.E) 
                && _playerInfo.HasKey)
            {
                _usedKey = true;
                _playerInfo.HasKey = false;
                _fireNpc.IsFree = true;
                StartCoroutine(InfoAfterFireAttack());
            }
        }

        //method controls what comes after letting npc free, displays info about key loss and disables cell collider
        private IEnumerator InfoAfterFireAttack()
        {
            instructionText.text = "You lost a key!";
            panel.SetActive(false);
            yield return new WaitForSeconds(1f);
            GetComponent<Collider2D>().enabled = false;
            instructionText.gameObject.SetActive(false);
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            //if player has key  instruction text is active
            if (_playerInfo.HasKey)
            {
                instructionText.gameObject.SetActive(false);
            }
            panel.SetActive(false);
        }
    }
}
