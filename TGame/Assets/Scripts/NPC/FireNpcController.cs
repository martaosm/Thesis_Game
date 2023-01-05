using System.Collections.Generic;
using Player;
using Scene;
using TMPro;
using UnityEngine;

namespace NPC
{
    public class FireNpcController : MonoBehaviour
    {
        private bool _isFree = false;
        public bool IsFree
        {
            get => _isFree;
            set => _isFree = value;
        }
        private int _encountersCount;
        public int EncountersCount
        {
            get => _encountersCount;
            set => _encountersCount = value;
        }
        private PlayerInfo _playerInfo;
        private Animator _animation;
        private CellDoorController _cellDoorController;
        [SerializeField] private Vector2 destination;
        [SerializeField] private float speed;
        [SerializeField] private List<string> convoWhenHasKey = new List<string>();
        [SerializeField] private List<string> convoWhenHasKeyNot = new List<string>();
        [SerializeField] private TextMeshProUGUI panelText;
        private static readonly int Attack = Animator.StringToHash("Attack");

        private void Start()
        {
            _playerInfo = FindObjectOfType<PlayerInfo>();
            _animation = GetComponent<Animator>();
            _cellDoorController = FindObjectOfType<CellDoorController>();
        }

        private void Update()
        {
            //if player opens the door then npc attacks them and steals the key
            if (_cellDoorController.DoorOpened)
            {
                _animation.SetBool(Attack, true);
                transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
            }
            
            //when npc reaches destination then game object is destroyed
            if (transform.position.x.Equals(destination.x))//transform.position.x == destination.x
            {
                Destroy(gameObject);
            }
            
        }
        
        /**
         * method controls interaction with player, dialog options depends on how many times player approaches npc
         */
        public void ConvoController()
        {
            switch (_playerInfo.HasKey)
            {
                case true:
                    if (_encountersCount == 1)
                    {
                        panelText.text = convoWhenHasKey[0];
                    }else if (_encountersCount > 1)
                    {
                        panelText.text = convoWhenHasKey[1];
                    }
                    break;
                case false:
                    if (_encountersCount == 1)
                    {
                        panelText.text = convoWhenHasKeyNot[0];
                    }else if (_encountersCount > 1)
                    {
                        panelText.text = convoWhenHasKeyNot[1];
                    }
                    break;
            }
        }

        //gets player prefs
        private void OnEnable()
        {
            _encountersCount = PlayerPrefs.GetInt("FireNpcEncounters");
            _isFree = PlayerPrefs.GetInt("IsFree") == 1;
        }

        //sets player prefs
        private void OnDisable()
        {
            PlayerPrefs.SetInt("FireNpcEncounters", _encountersCount);
            PlayerPrefs.SetInt("IsFree", _isFree ? 1 : 0);
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            //if npc collides with ramp object then is destroyed
            if (col.gameObject.CompareTag("Ramp"))
            {
                _animation.SetBool(Attack, false);
                Destroy(gameObject);
            }
        }
    }
}
