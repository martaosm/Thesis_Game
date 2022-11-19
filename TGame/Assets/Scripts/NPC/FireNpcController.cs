using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Player;
using Scene;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        private BoxCollider2D _collider;
        private Animator _animation;
        private SpriteRenderer _fireNpc;
        private Rigidbody2D _rb;
        private CellDoorController _cellDoorController;
        private Transform _attackPoint;
        [SerializeField] private float speed;
        [SerializeField] private LayerMask jumpGround;
        [SerializeField] private List<string> convoWhenHasKey = new List<string>();
        [SerializeField] private List<string> convoWhenHasKeyNot = new List<string>();
        [SerializeField] private TextMeshProUGUI panelText;
        [SerializeField] private GameObject player;
        [SerializeField] private Transform leftPoint;
        [SerializeField] private Transform rightPoint;
        [SerializeField] private Transform centerPoint;
        [SerializeField] private GameObject triggerArea;

        private void Start()
        {
            _playerInfo = FindObjectOfType<PlayerInfo>();
            _collider = GetComponent<BoxCollider2D>();
            _animation = GetComponent<Animator>();
            _fireNpc = GetComponent<SpriteRenderer>();
            //_rb = GetComponent<Rigidbody2D>();
            _cellDoorController = FindObjectOfType<CellDoorController>();
        }

        private void Update()
        {
            if (SceneManager.GetActiveScene().name == "Chapter1")
            {
                Physics2D.IgnoreCollision(_collider, player.GetComponent<BoxCollider2D>());
                if (_cellDoorController.DoorOpened)
                {
                    _animation.SetBool("Attack", true);
                    //_rb.velocity = new Vector2(speed, _rb.velocity.y);
                }
            }

            if (player.transform.position.x > gameObject.transform.position.x)
            {
                gameObject.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
                //_attackPoint = rightPoint;
            }
            else if (player.transform.position.x < gameObject.transform.position.x)
            {
                gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                //_attackPoint = leftPoint;
            }
                //UpdateAnimation();
        }

        private void UpdateAnimation()
        {
            if (_rb.velocity.magnitude < 0f )
            { 
                _fireNpc.flipX = false;
            }
            else if (_rb.velocity.magnitude > 0f )
            {
                _fireNpc.flipX = true;
            }
            if (_rb.velocity.magnitude < 0f && IsGrounded())
            {
                //_animation.Play("FireWalk");
                _fireNpc.flipX = false;
            }
            else if (_rb.velocity.magnitude > 0f && IsGrounded())
            {
                //_animation.Play("FireWalk");
                _fireNpc.flipX = true;
            }
            else if(_rb.velocity.magnitude==0f && IsGrounded() )
            {
                _animation.Play("FireNpcIdle");
            }

            if (!IsGrounded())
            {
                _animation.Play("FireJump");
            }
            
        }

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

        private void OnEnable()
        {
            _encountersCount = PlayerPrefs.GetInt("FireNpcEncounters");
            _isFree = PlayerPrefs.GetInt("IsFree") == 1 ? true : false;
            TriggerAreaController.OnAttack += FireAttack;
        }

        private void OnDisable()
        {
            PlayerPrefs.SetInt("FireNpcEncounters", _encountersCount);
            PlayerPrefs.SetInt("IsFree", _isFree ? 1 : 0);
            TriggerAreaController.OnAttack -= FireAttack;
        }
        
        private bool IsGrounded()
        {
            return Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0f, Vector2.down, .1f, jumpGround);
        }

        /*private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Ramp"))
            {
                Destroy(gameObject);
            }
        }*/

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("Ramp"))
            {
                _animation.SetBool("Attack", false);
                Destroy(gameObject);
            }

            if (col.gameObject.GetComponent<PlayerInfo>())
            {
                if (_animation.GetBool("Attack") == true)
                {
                    _animation.SetBool("Attack", false);
                    gameObject.tag = "FireNpc";
                }
            }
        }

        /*private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.GetComponent<PlayerInfo>())
            {
                //_animation.SetBool("Attack", true);
                gameObject.tag = "enemyWeapon";
                //StartCoroutine(FireAttack());
                //transform.position = Vector2.MoveTowards(transform.position, col.gameObject.transform.position, 7 * Time.deltaTime);
                /*gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position,
                    col.gameObject.transform.position, 10 * Time.deltaTime);#1#
            }
        }*/

        /*private void OnTriggerStay2D(Collider2D other)
        {
            if (other.GetComponent<PlayerInfo>())
            {
                Debug.Log("hahaha1");
                //_animation.SetBool("Attack", true);
                //gameObject.tag = "enemyWeapon";
                //StartCoroutine(FireAttack());
                //transform.position = Vector2.MoveTowards(transform.position, col.gameObject.transform.position, 7 * Time.deltaTime);
                /*gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position,
                    col.gameObject.transform.position, 10 * Time.deltaTime);#1#
            }
        }*/

         private void FireAttack()
         {
             _attackPoint = leftPoint;
            Debug.Log(_attackPoint);
            gameObject.tag = "enemyWeapon";
            transform.position = Vector2.MoveTowards(transform.position, _attackPoint.localPosition, 7 * Time.deltaTime);
            //yield return new WaitForSeconds(1f);
            //_attackPoint = centerPoint;
            //transform.position = Vector2.MoveTowards(transform.position, _attackPoint.position, 7 * Time.deltaTime);
        }
        
    }
}
