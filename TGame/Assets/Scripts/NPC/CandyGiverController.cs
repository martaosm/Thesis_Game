using System;
using System.Collections;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NPC
{
    /**
     * Controls behaviour of npc in Chapter3 scene
     */
    public class CandyGiverController : MonoBehaviour
    {
        private float _life = 20f;
        public float Life
        {
            get => _life;
            set => _life = value;
        }

        private bool _candyGiverDealt;
        private bool _guardDone;
        private bool _isGuardAnEnemy;
        private Animator _animator;
        private PlayerInfo _playerInfo;
        private bool _isPlayerAnEnemy;
        private bool _answerGiven;
        private bool _actionAfterAns = false;
        private String _answer = "";
        [SerializeField] private Slider slider;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI panelText;
        [SerializeField] private TextMeshProUGUI instructions;
        [SerializeField] private Collider2D collider;
        [SerializeField] private Collider2D trigger;
        [SerializeField] private GameObject closedChamber;
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Death0 = Animator.StringToHash("Death0");

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

            //starts npc "death" sequence
            if (_life <= 0)
            {
                StartCoroutine(CandyGiverDeath());
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
        
            //depending on answer of the player, if "no" the player becomes an enemy, if "yes" player is given hp (it can be negative or positive)
            if (_answer != "" && _answer != "answer given")
            {
                switch (_answer)
                {
                    case "no":
                        _isPlayerAnEnemy = true;
                        closedChamber.SetActive(true);
                        break;
                    case "yes":
                        _isPlayerAnEnemy = false;
                        break;
                }
            }
            
            //before giving an answer, waiting for input from player
            if (!_answerGiven)
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
        }
    
        //"death" sequence, after defeat chamber is opened and npc disappears 
        private IEnumerator CandyGiverDeath()
        {
            _life++;
            _animator.SetBool(Death0, true);
            _candyGiverDealt = true;
            _answer = "answer given";
            closedChamber.SetActive(false);
            yield return new WaitForSeconds(1.5f);
            gameObject.SetActive(false);
        }
    
        private void OnTriggerEnter2D(Collider2D col)
        {
            //if player enters trigger text panel is activated
            if (col.gameObject.GetComponent<PlayerInfo>())
            {
                panel.SetActive(true);
                panelText.text = "Hey, you want a special kind of candy?";
                instructions.text = "[Y] yes / [N] no";
                instructions.gameObject.SetActive(true);
            }

            //gets player prefs, needed to determine dialog option
            _guardDone = PlayerPrefs.GetInt("DemonGuardDone") == 1;
            _isGuardAnEnemy = PlayerPrefs.GetInt("IsGuardAnEnemy") == 1;
        }
        
        private void OnTriggerStay2D(Collider2D other)
        {
            //depending on earlier actions player will gain or loose HP
            if (other.gameObject.GetComponent<PlayerInfo>())
            {
                if (_answerGiven)
                {
                    if (_guardDone)
                    {
                        if (!_isGuardAnEnemy && !_actionAfterAns)
                        {
                            ActionAfterAnswer(-50, 0);
                            _actionAfterAns = true;
                        }

                        if (_isGuardAnEnemy && !_actionAfterAns)
                        {
                            ActionAfterAnswer(50, 1);
                            _actionAfterAns = true;
                        }
                    }
                    else if (!_guardDone && !_actionAfterAns)
                    {
                        ActionAfterAnswer(-50, 0);
                        _actionAfterAns = true;
                    }
                }
            }
        }

        //controls actions after giving an answer, "no" - attacks player, "yes" - player gains or looses HP
        private void ActionAfterAnswer(int hp, int dialog)
        {
            switch (_answer)
            {
                case "no":
                {
                    collider.enabled = true;
                    trigger.enabled = false;
                    panel.SetActive(false);
                    instructions.gameObject.SetActive(false);
                    if (_isPlayerAnEnemy)
                    {
                        gameObject.layer = 10;
                        if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                        {
                            _animator.SetTrigger(Attack);
                        }
                    }

                    break;
                }
                case "yes":
                    StartCoroutine(PlayerTakesCandy(hp, dialog));
                    break;
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            //text panel is deactivated 
            if (other.gameObject.GetComponent<PlayerInfo>())
            {
                panel.SetActive(false);
                instructions.gameObject.SetActive(false);
            }
        }
    
        //if answer was "yes" then player gains or looses HP
        private IEnumerator PlayerTakesCandy(int hp, int dialog)
        {
            const string s1 = "You like it? That kind of candy will increase your health by 50 HP. Thank you for tasting it! Now I gotta go, bye!";
            const string s2 = "Haha. It was a trap! Now you lost 50 HP!MUHAHAHAHAHA";
            instructions.gameObject.SetActive(false);
            _playerInfo.Health += hp;//_health
            panelText.text = dialog == 1 ? s1 : s2;
            yield return new WaitForSeconds(4f);
            _animator.SetBool(Death0, true);
            panel.SetActive(false);
            _candyGiverDealt = true;
            yield return new WaitForSeconds(1.5f);
            gameObject.SetActive(false);
        }

        //if player interacted with nps then later npc is not active in the scene
        private void OnEnable()
        {
            _candyGiverDealt = PlayerPrefs.GetInt("CandyGiverDealt") == 1;
            if (_candyGiverDealt)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            PlayerPrefs.SetInt("CandyGiverDealt", _candyGiverDealt ? 1 : 0);
        }
    }
}
