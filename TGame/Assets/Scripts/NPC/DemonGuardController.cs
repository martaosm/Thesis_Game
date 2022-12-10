using System;
using System.Collections;
using System.Collections.Generic;
using ChaptersControllers;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NPC
{
    public class DemonGuardController : MonoBehaviour
    {
        private int _guardEncountersCount;

        private float _life = 50f;
        public float Life
        {
            get => _life;
            set => _life = value;
        }

        private int _randomConvo;
        private bool _isGuardAnEnemy;
        private String _answer = "";
        private bool _isPlayerAnEnemy;
        private bool _answerGiven;
        private Animator _animator;
        private PlayerInfo _playerInfo;
        [SerializeField] private Slider slider;
        [SerializeField] private GameObject player;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private Transform triggerArea;
        [SerializeField] private Vector2 triggerAreaSize;
        [SerializeField] private Vector2 dgCheckpoint;
        [SerializeField] private List<string> conversation;
        [SerializeField] private List<string> conversationTruth;
        [SerializeField] private List<string> conversationLies;
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI panelText;
        [SerializeField] private TextMeshProUGUI instructions; 
        [SerializeField] private GameObject closedChamber;
        private static readonly int Attack = Animator.StringToHash("Attack");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _playerInfo = FindObjectOfType<PlayerInfo>();
        }

        private void Update()
        {
            //controls health bar
            if (_life <= 0)
            {
                slider.gameObject.SetActive(false);
            }
            else
            {
                slider.value = _life;
            }
        
            //stop attacking when player's health equals or is under 0
            if (_playerInfo.Health <= 0)//_health
            {
                _isPlayerAnEnemy = false;
            }
        
            //depending on answer of the player, if "yes" the player becomes an enemy and guard is set as an enemy, chamber is closed
            //if "no" player is left alone or given a clue, guard doesn't become an enemy
            if (_answer != "" && _answer != "answer given")
            {
                switch (_answer)
                {
                    case "yes":
                        _isPlayerAnEnemy = true;
                        _isGuardAnEnemy = true;
                        closedChamber.SetActive(true);
                        StartCoroutine(ConversationAfterAnswer());
                        _answer = "answer given";
                        break;
                    case "no":
                        _isPlayerAnEnemy = false;
                        _isGuardAnEnemy = false;
                        StartCoroutine(ConversationAfterAnswer());
                        _answer = "answer given";
                        break;
                }
            }
            
            //before giving an answer, waiting for input from player
            if (_guardEncountersCount > 1 && !_answerGiven)
            {
                if (Input.GetKey(KeyCode.Y))
                {
                    _answer = "yes";
                    _answerGiven = true;
                }

                if (Input.GetKey(KeyCode.N))
                {
                    _answer = "no";
                    _answerGiven = true;
                }
            }

            //after defeat chamber is opened and guard leaves
            if (_life <= 10)
            {
                _isPlayerAnEnemy = false;
                closedChamber.SetActive(false);
                StartCoroutine(ConversationAfterDefeat());
            }
        
            //npc faces player all the time
            if (player.transform.position.x > gameObject.transform.position.x)
            {
                gameObject.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
            }
            else if (player.transform.position.x < gameObject.transform.position.x)
            {
                gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            }
        }

        //dialog controller after giving an answer, later dialog depends on player's answer
        private IEnumerator ConversationAfterAnswer()
        {
            instructions.gameObject.SetActive(false);
            panelText.text = _answer == "yes" ? conversationTruth[0] : conversationLies[_randomConvo];
            yield return new WaitForSeconds(4f);
            FindObjectOfType<Chapter2Controller>().demonGuardDone = true;
            panel.SetActive(false);
        }

        //after player defeats npc, npc leaves 
        private IEnumerator ConversationAfterDefeat()
        {
            panel.SetActive(true);
            panelText.text = conversationTruth[1];
            yield return new WaitForSeconds(4f);
            transform.position = Vector2.MoveTowards(transform.position, dgCheckpoint, 7 * Time.deltaTime);
            yield return new WaitForSeconds(2f);
            gameObject.SetActive(false);
            panel.SetActive(false);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.GetComponent<PlayerInfo>())
            {
                ConversationController();
            
                if (Input.GetKey(KeyCode.E) && !_answerGiven)
                {
                    _guardEncountersCount++;
                }

                if (_answer == "answer given")
                {
                    //if player becomes an enemy, npc attacks player
                    if (_isPlayerAnEnemy)
                    {
                        gameObject.layer = 10;
                        if (Physics2D.OverlapBoxAll(triggerArea.position, triggerAreaSize, playerLayer).Length > 0 &&
                            _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                        {
                            _animator.SetTrigger(Attack);
                        }
                    }
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            //text panel is set active, depending on player's health the method decides on giving them a clue
            if (col.gameObject.GetComponent<PlayerInfo>() && !_answerGiven)
            {
                _randomConvo = _playerInfo.Health < _playerInfo.maxHealth/2 ? 0 : 1;//_health
                _guardEncountersCount++;
                panel.SetActive(true);
                instructions.gameObject.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            //text panel is deactivated
            if (other.gameObject.GetComponent<PlayerInfo>() && !_answerGiven)
            {
                panel.SetActive(false);
                instructions.gameObject.SetActive(false);
            }
        }

        //interaction controller, dialog depends on how many times player approaches the npc
        private void ConversationController()
        {
            if (_guardEncountersCount == 1)
            {
                instructions.text = "Next [E]";
                panelText.text = conversation[0];
            }
            if (_guardEncountersCount > 1 && !_answerGiven)
            {
                instructions.text = "[Y] yes / [N] no";
                panelText.text = conversation[1];
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(triggerArea.position, triggerAreaSize);
        }

        private void OnEnable()
        {
            _guardEncountersCount = PlayerPrefs.GetInt("GuardEncounters");
            _isGuardAnEnemy = PlayerPrefs.GetInt("IsGuardAnEnemy") == 1;
        }

        private void OnDisable()
        {
            PlayerPrefs.SetInt("GuardEncounters", _guardEncountersCount);
            PlayerPrefs.SetInt("IsGuardAnEnemy", _isGuardAnEnemy ? 1 : 0);

        }
    }
}
