using System;
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
        [SerializeField] private float speed;
        [SerializeField] private GameObject cellDoor;
        [SerializeField] private GameObject cellWall;
        [SerializeField] private TextMeshProUGUI instructionText;
        [SerializeField] private GameObject panel;


        private void OnEnable()
        {
            _playerInfo = FindObjectOfType<PlayerInfo>();
            _fireNpc = FindObjectOfType<FireNpcController>();
        }

        private void Update()
        {
            if (_usedKey)
            {
                cellDoor.transform.position = Vector2.MoveTowards(transform.position, cellWall.transform.position, speed);
                cellDoor.GetComponent<Collider2D>().enabled = false;
                _usedKey = false;
                _doorOpened = true;
                _playerInfo.InputEnabled = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
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
            if (other.gameObject.GetComponent<PlayerInfo>() && Input.GetKey(KeyCode.E) && _playerInfo.HasKey)
            {
                _usedKey = true;
                _playerInfo.HasKey = false;
                GetComponent<Collider2D>().enabled = false;
                instructionText.gameObject.SetActive(false);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_playerInfo.HasKey)
            {
                instructionText.gameObject.SetActive(false);
                
            }
            panel.SetActive(false);
        }
    }
}
