using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DemonGuardController : MonoBehaviour
{
    private int _guardEncountersCount;
    public int GuardEncountersCount
    {
        get => _guardEncountersCount;
        set => _guardEncountersCount = value;
    }

    private float _life = 50f;
    public float Life
    {
        get => _life;
        set => _life = value;
    }

    public bool _isGuardAnEnemy;
    private String _answer = "";
    private bool _isPlayerAnEnemy;
    private bool _answerGiven;
    private Animator _animator;
    private PlayerInfo _playerInfo;
    [SerializeField] private Slider _slider;
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
        if (_life <= 0)
        {
            _slider.gameObject.SetActive(false);
        }
        else
        {
            _slider.value = _life;
        }
        
        if (_playerInfo._health <= 0)
        {
            _isPlayerAnEnemy = false;
        }
        
        if (_answer != "" && _answer != "answer given")
        {
            switch (_answer)
            {
                case "yes":
                    _isPlayerAnEnemy = true;
                    _isGuardAnEnemy = true;
                    closedChamber.SetActive(true);
                    StartCoroutine(ConversationAfterAnswer());
                    break;
                case "no":
                    _isPlayerAnEnemy = false;
                    _isGuardAnEnemy = false;
                    StartCoroutine(ConversationAfterAnswer());
                    //TODO: gain an ally and advance further 
                    break;
            }
        }
        if (_guardEncountersCount > 1 && !_answerGiven)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                _answer = "yes";
                _answerGiven = true;
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                _answer = "no";
                _answerGiven = true;
            }
        }

        if (_life <= 10)
        {
            _isPlayerAnEnemy = false;
            closedChamber.SetActive(false);
            StartCoroutine(ConversationAfterDefeat());
        }
        
        if (player.transform.position.x > gameObject.transform.position.x)
        {
            gameObject.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }
        else if (player.transform.position.x < gameObject.transform.position.x)
        {
            gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
    }

    IEnumerator ConversationAfterAnswer()
    {
        instructions.gameObject.SetActive(false);
        panelText.text = _answer == "yes" ? conversationTruth[0] : conversationLies[0];
        yield return new WaitForSeconds(4f);
        FindObjectOfType<Chapter2Controller>().demonGuardDone = true;
        panel.SetActive(false);
        _answer = "answer given";
    }

    IEnumerator ConversationAfterDefeat()
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
            
            if (Input.GetKeyDown(KeyCode.E) && !_answerGiven)
            {
                _guardEncountersCount++;
            }

            if (_answer == "answer given")
            {
                //panel.SetActive(false);
                //instructions.gameObject.SetActive(false);
                if (_isPlayerAnEnemy)
                {
                    gameObject.layer = 11;
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
        if (col.gameObject.GetComponent<PlayerInfo>() && !_answerGiven)
        {
            _guardEncountersCount++;
            panel.SetActive(true);
            instructions.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerInfo>() && !_answerGiven)
        {
            panel.SetActive(false);
            instructions.gameObject.SetActive(false);
        }
    }

    private void ConversationController()
    {
        if (_guardEncountersCount == 1)
        {
            instructions.text = "Next [E]";
            panelText.text = conversation[0];
        }
        if (_guardEncountersCount > 1)
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
