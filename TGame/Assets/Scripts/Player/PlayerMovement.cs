using System;
using System.Collections;
using NPC;
using Scene;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {

        [Header("For WallSliding")] 
        [SerializeField] private float wallSlideSpeed = 0;
        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private LayerMask fireNpcLayer;
        [SerializeField] private Transform wallCheckPoint;
        [SerializeField] private Vector2 wallCheckSize;
        private bool _isTouchingWall;
        private bool _isWallSliding;
        
        private bool _isBeingAttacked;
        private PlayerInfo _playerInfo;
        private Rigidbody2D _rb;
        private SpriteRenderer _player;
        private Animator _animation;
        private BoxCollider2D _collider;
        private float _dirX = 0f;
        private MovementState _state;
        private bool _canMove = true;
        private SceneController _sceneController;
        [SerializeField] private float _jumpForce = 10f;
        [SerializeField] private float _speed = 5f;
        [SerializeField] private LayerMask jumpGround;
        //[SerializeField] private GameObject fireNpc;
        private static readonly int State = Animator.StringToHash("state");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Ladder = Animator.StringToHash("Ladder");

        private enum MovementState
        {
            Idle,
            Run,
            Jump,
            Fall,
            WallSlide,
            HurtFromFire
        };
        
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _player = GetComponent<SpriteRenderer>();
            _animation = GetComponent<Animator>();
            _collider = GetComponent<BoxCollider2D>();
            _playerInfo = GetComponent<PlayerInfo>();
            _sceneController = FindObjectOfType<SceneController>();
            
        }

        private void Update()
        {
            if (_canMove && _playerInfo.InputEnabled)
            {
                _dirX = Input.GetAxisRaw("Horizontal");
                _rb.velocity = new Vector2(_dirX * _speed, _rb.velocity.y);
                if (Input.GetButtonDown("Jump") && IsGrounded() && _playerInfo.InputEnabled)
                {
                    _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
                }
            }

            IsAttackedByFire();
            UpdateAnimation();
            PlayerAttack(IsGrounded());
            WallSlide();
        }

        private void UpdateAnimation()
        {
            if (_dirX > 0f )
            {
                _player.flipX = false;
            }
            else if (_dirX < 0f )
            {
                _player.flipX = true;
            }
            if (_dirX > 0f && IsGrounded())
            {
                _state = MovementState.Run;
                _player.flipX = false;
            }
            else if (_dirX < 0f && IsGrounded())
            {
                _state = MovementState.Run;
                _player.flipX = true;
            }
            else if(_dirX==0f && IsGrounded() )
            {
                _state = MovementState.Idle;
            }
            else
            {
                _state = MovementState.Fall;
            }
            
            if (_rb.velocity.y > .1f)
            {
                _state = MovementState.Jump;
            }
            else if (_rb.velocity.y < -.1f)
            {
                _state = MovementState.Fall;
            }

            if (IsTouchingWall() && !IsGrounded())
            {
                _state = MovementState.WallSlide;
            }

            if (_isBeingAttacked)
            {
                StartCoroutine(PlayerHurtFromFire());
            }
            
            _animation.SetInteger(State, (int)_state);
        }
        
        private void PlayerAttack(bool grounded)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && grounded && _playerInfo.InputEnabled)
            {
                StartCoroutine(PlayerExecuteAttack());
                _rb.velocity = new Vector2(0, _rb.velocity.y);
                _canMove = false;
                _playerInfo.SetIsAttacking(true);
            }
        }

        IEnumerator PlayerExecuteAttack()
        {
            _animation.SetTrigger(Attack);
            yield return new WaitForSeconds(1f);
            _canMove = true;
            _playerInfo.SetIsAttacking(false);
        }

        private void WallSlide()
        {
            if (_isTouchingWall && !IsGrounded() && _rb.velocity.y < -.1f)
            {
                _isWallSliding = true;
            }
            else
            {
                _isWallSliding = false;
            }

            if (_isWallSliding)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, wallSlideSpeed);
            }
        }

        private bool IsGrounded()
        {
            return Physics2D.BoxCast(_collider.bounds.center, _collider.bounds.size, 0f, Vector2.down, .1f, jumpGround);
        }
        
        private bool IsTouchingWall()
        {
            _isTouchingWall = Physics2D.OverlapBox(wallCheckPoint.position, wallCheckSize, 0, wallLayer);
            return _isTouchingWall;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color=Color.blue;
            Gizmos.DrawCube(wallCheckPoint.position, wallCheckSize);
        }

        private bool IsAttackedByFire()
        {
            if (Physics2D.OverlapBox(wallCheckPoint.position, wallCheckSize, 0, fireNpcLayer))
            {
                _isBeingAttacked = true;
            }
            return _isBeingAttacked;
        }
        
        IEnumerator PlayerHurtFromFire()
        {
            _state = MovementState.HurtFromFire;
            _animation.SetInteger(State, (int)_state);
            yield return new WaitForSeconds(2f);
            _state = MovementState.Idle;
            _animation.SetInteger(State, (int)_state);
            _isBeingAttacked = false;
            _playerInfo.InputEnabled = true;
        }
    }
}
