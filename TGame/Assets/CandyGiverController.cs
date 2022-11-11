using System;
using Player;
using TMPro;
using UnityEngine;

public class CandyGiverController : MonoBehaviour
{
    private Animator _animator;
    private PlayerInfo _playerInfo;
    private bool _isPlayerAnEnemy;
    private bool _answerGiven;
    private String _answer = "";
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject hitBox;
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI panelText;
    [SerializeField] private TextMeshProUGUI instructions;
    [SerializeField] private Collider2D collider;
    [SerializeField] private GameObject closedChamber;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _playerInfo = FindObjectOfType<PlayerInfo>();
    }

    private void Update()
    {
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
                //panel.SetActive(false);
                //instructions.gameObject.SetActive(false);
                /*if (_isPlayerAnEnemy)
                {
                    gameObject.layer = 11;
                    if (Physics2D.OverlapBoxAll(triggerArea.position, triggerAreaSize, playerLayer).Length > 0 &&
                        _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                        _animator.SetTrigger(Attack);
                    }
                }*/
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
}
