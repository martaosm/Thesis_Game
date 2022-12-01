using System;
using System.Collections;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CandyGiverController : MonoBehaviour
{
    private float _life = 20f;
    public float Life
    {
        get => _life;
        set => _life = value;
    }

    private bool _candyGiverDealt;
    private Animator _animator;
    private PlayerInfo _playerInfo;
    private bool _isPlayerAnEnemy;
    private bool _answerGiven;
    private String _answer = "";
    [SerializeField] private Slider _slider;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject hitBox;
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI panelText;
    [SerializeField] private TextMeshProUGUI instructions;
    [SerializeField] private Collider2D collider;
    [SerializeField] private Collider2D trigger;
    [SerializeField] private GameObject closedChamber;
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Death = Animator.StringToHash("Death");
    private static readonly int Death0 = Animator.StringToHash("Death0");

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

        if (_life <= 0)
        {
            StartCoroutine(CandyGiverDeath());
            closedChamber.SetActive(false);
        }
        
        if (player.transform.position.x > gameObject.transform.position.x)
        {
            gameObject.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }
        else if (player.transform.position.x < gameObject.transform.position.x)
        {
            gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
        
        if (_answer != "" && _answer != "answer given")
        {
            switch (_answer)
            {
                case "no":
                    _isPlayerAnEnemy = true;
                    closedChamber.SetActive(true);
                    //TODO: FIGHT
                    break;
                case "yes":
                    _isPlayerAnEnemy = false;
                    //TODO: take and gain
                    break;
            }
        }
        if (!_answerGiven)
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
    }
    
    IEnumerator CandyGiverDeath()
    {
        _life++;
        _animator.SetBool(Death0, true);
        _candyGiverDealt = true;
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<PlayerInfo>())
        {
            panel.SetActive(true);
            panelText.text = "Hey, you want a special kind of candy?";
            instructions.text = "[Y] yes / [N] no";
            instructions.gameObject.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerInfo>())
        {
            if (_answer == "no")
            {
                collider.enabled = true;
                trigger.enabled = false;
                panel.SetActive(false);
                instructions.gameObject.SetActive(false);
                if (_isPlayerAnEnemy)
                {
                    gameObject.layer = 12;
                    if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                        _animator.SetTrigger(Attack);
                    }
                }
            }

            if (_answer == "yes")
            {
                StartCoroutine(PlayerTakesCandy());
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerInfo>())
        {
            panel.SetActive(false);
            instructions.gameObject.SetActive(false);
        }
    }
    
    IEnumerator PlayerTakesCandy()
    {
        instructions.gameObject.SetActive(false);
        _playerInfo._health += 50;
        panelText.text =
            "You like it? That kind of candy will increase your health by 50 HP. Thank you for tasting it! Now I gotta go, bye!";
        yield return new WaitForSeconds(4f);
        //_life++;
        _animator.SetBool(Death0, true);
        panel.SetActive(false);
        _candyGiverDealt = true;
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }

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
