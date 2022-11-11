using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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
        private BoxCollider2D _collider;
        private Animator _animation;
        private SpriteRenderer _fireNpc;
        private Rigidbody2D _rb;
        private CellDoorController _cellDoorController;
        [SerializeField] private float speed;
        [SerializeField] private LayerMask jumpGround;
        [SerializeField] private List<string> convoWhenHasKey = new List<string>();
        [SerializeField] private List<string> convoWhenHasKeyNot = new List<string>();
        [SerializeField] private TextMeshProUGUI panelText;
        [SerializeField] private GameObject player;

        private void Start()
        {
            _playerInfo = FindObjectOfType<PlayerInfo>();
            _collider = GetComponent<BoxCollider2D>();
            _animation = GetComponent<Animator>();
            _fireNpc = GetComponent<SpriteRenderer>();
            _rb = GetComponent<Rigidbody2D>();
            _cellDoorController = FindObjectOfType<CellDoorController>();
        }

        private void Update()
        {
            Physics2D.IgnoreCollision(_collider, player.GetComponent<BoxCollider2D>());
            if (_cellDoorController.DoorOpened)
            {
                _rb.velocity = new Vector2(speed, _rb.velocity.y);
            }
            UpdateAnimation();
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
                _animation.Play("FireWalk");
                _fireNpc.flipX = false;
            }
            else if (_rb.velocity.magnitude > 0f && IsGrounded())
            {
                _animation.Play("FireWalk");
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
        }

        private void OnDisable()
        {
            PlayerPrefs.SetInt("FireNpcEncounters", _encountersCount);
            PlayerPrefs.SetInt("IsFree", _isFree ? 1 : 0);
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
                Destroy(gameObject);
            }
        }
    }
}
