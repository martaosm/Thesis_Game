using System;
using System.Collections;
using NPC;
using NPC.Enemies;
using Scene;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {

        [Header("For WallSliding")] 
        [SerializeField] private float wallSlideSpeed = 0;
        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private Transform wallCheckPoint;
        [SerializeField] private Vector2 wallCheckSize;
        private bool _isTouchingWall;
        private bool _isWallSliding;
        
        private bool _isBeingAttacked;
        private bool _isPlayerCrouching;
        private PlayerInfo _playerInfo;
        private Rigidbody2D _rb;
        private SpriteRenderer _player;
        private Animator _animation;
        private BoxCollider2D _collider;
        private float _dirX = 0f;
        private MovementState _state;
        private bool _canMove = true;
        private float _attackRate = 2f;
        private float _nextAttackTime = 0f;
        private SceneController _sceneController;
        [SerializeField] private float _jumpForce = 10f;
        [SerializeField] private float _speed = 5f;
        [SerializeField] private LayerMask jumpGround;
        [SerializeField] private LayerMask fireNpcLayer;
        [SerializeField] private Transform attackPoint;
        [SerializeField] private float attackRange;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private LayerMask enemyDemonGuard;
        [SerializeField] private LayerMask enemyCandyGiver;
        [SerializeField] private LayerMask kleptoNpc;
        public delegate void KleptoDeath();
        public static event KleptoDeath OnKleptoDeath;
        private static readonly int State = Animator.StringToHash("state");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int HurtFromFire = Animator.StringToHash("HurtFromFire");
        private static readonly int Crouch = Animator.StringToHash("Crouch");

        private enum MovementState
        {
            Idle,
            Run,
            Jump,
            Fall,
            WallSlide,
            Crouch
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
                if (_isPlayerCrouching)
                {
                    _rb.velocity = new Vector2(0, 0);
                }
                
                if (!_isPlayerCrouching)
                {
                    _dirX = Input.GetAxisRaw("Horizontal");
                    _rb.velocity = new Vector2(_dirX * _speed, _rb.velocity.y);
                //}

                //if (!_isPlayerCrouching)
                //{
                    if (Input.GetButtonDown("Jump") && IsGrounded() && _playerInfo.InputEnabled)
                    {
                        _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
                    }
                }
            }

            IsAttackedByFire();
            UpdateAnimation();
            PlayerAttack(IsGrounded());
            WallSlide();

            if (_playerInfo._health == 0)
            {
                print("you ded");
                //_collider.enabled = false;
                _animation.Play("PlayerDeath");
                StartCoroutine(ChangeToGameOverScene());
                _playerInfo._health = -1;
            }
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
            else if(_dirX==0f && IsGrounded() && !_isPlayerCrouching )
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
                _animation.SetTrigger(HurtFromFire);
                _isBeingAttacked = false;
                _playerInfo.InputEnabled = true;
            }

            if (Input.GetKey(KeyCode.C))
            {
                _animation.SetBool(Crouch, true);
                _state = MovementState.Crouch;
                _isPlayerCrouching = true;
            }

            if (Input.GetKeyUp(KeyCode.C))
            {
                _animation.SetBool(Crouch, false);
                _isPlayerCrouching = false;
            }

            _animation.SetInteger(State, (int)_state);
        }
        
        private void PlayerAttack(bool grounded)
        {
            if (Time.time >= _nextAttackTime)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && grounded && _playerInfo.InputEnabled)
                {
                    StartCoroutine(PlayerExecuteAttack());
                    _rb.velocity = new Vector2(0, _rb.velocity.y);
                    _canMove = false;
                    _playerInfo.SetIsAttacking(true);
                    _nextAttackTime = Time.time + 1f / _attackRate;
                }
            }
            
        }

        IEnumerator PlayerExecuteAttack()
        {
            _animation.SetTrigger(Attack);
            //skeletons
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
            foreach (var hitEnemy in hitEnemies)
            {
                hitEnemy.gameObject.GetComponent<EnemyController>().Life -= 2;
                if (hitEnemy.gameObject.GetComponent<EnemyController>().Life <= 0)
                {
                    hitEnemy.gameObject.GetComponent<Animator>().SetTrigger(Death);
                    StartCoroutine(hitEnemy.gameObject.GetComponent<EnemyController>().DeadBodyDestroy());
                    break;
                }
                hitEnemy.gameObject.GetComponent<Animator>().SetTrigger(Hit);
            }
            
            //demon guard
            Collider2D[] hitGuard = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyDemonGuard);
            foreach (var hit in hitGuard)
            {
                hit.gameObject.GetComponent<DemonGuardController>().Life -= 5;
                print(hit.gameObject.GetComponent<DemonGuardController>().Life);//remove
                if (hit.gameObject.GetComponent<DemonGuardController>().Life <= 10)
                {
                    //hit.gameObject.GetComponent<Animator>().SetTrigger(Death);
                    //StartCoroutine(hit.gameObject.GetComponent<EnemyController>().DeadBodyDestroy());
                    //print("ok");
                    break;
                }
                hit.gameObject.GetComponent<Animator>().SetTrigger("Hurt");
            }
            
            //candy giver
            //TODO: fight that creep
            Collider2D[] hitCandy = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyCandyGiver);
            foreach (var hit in hitCandy)
            {
                hit.gameObject.GetComponent<CandyGiverController>().Life -= 5;
                print(hit.gameObject.GetComponent<CandyGiverController>().Life);//remove
                if (hit.gameObject.GetComponent<CandyGiverController>().Life <= 0)
                {
                    //hit.gameObject.GetComponent<Animator>().SetTrigger(Death);
                    //StartCoroutine(hit.gameObject.GetComponent<EnemyController>().DeadBodyDestroy());
                    print("ok");//6A3434 //FFFFFF
                    break;
                }
                hit.gameObject.GetComponent<Animator>().SetTrigger("Hurt");
                gameObject.GetComponent<SpriteRenderer>().color = new Color(106f, 52f, 52f, 255); 
                gameObject.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 255); 
            }
            
            //klepto npc
            Collider2D[] hitFire = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, kleptoNpc);
            foreach (var hitEnemy in hitFire)
            {
                hitEnemy.gameObject.GetComponent<KleptomaniacController>()._health -= 5f;
                if (hitEnemy.gameObject.GetComponent<KleptomaniacController>()._health <= 0)
                {
                    hitEnemy.gameObject.GetComponent<Animator>().SetTrigger("Death");
                    OnKleptoDeath?.Invoke();
                }
                /*if (hitEnemy.gameObject.GetComponent<EnemyController>().Life <= 0)
                {
                    hitEnemy.gameObject.GetComponent<Animator>().SetTrigger(Death);
                    StartCoroutine(hitEnemy.gameObject.GetComponent<EnemyController>().DeadBodyDestroy());
                    break;
                }*/
                
            }
            
            
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

        private bool IsAttackedByFire()
        {
            if (Physics2D.OverlapBox(wallCheckPoint.position, wallCheckSize, 0, fireNpcLayer))
            {
                _isBeingAttacked = true;
            }
            return _isBeingAttacked;
        }

        IEnumerator ChangeToGameOverScene()
        {
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene("GameOverScene");
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("enemyWeapon"))
            {
                _playerInfo.InputEnabled = false;
                _playerInfo._health -= 10;
                _animation.Play("PlayerHurt");
                
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("enemyWeapon"))
            {
                _playerInfo.InputEnabled = false;
                _playerInfo._health -= 10;
                _animation.Play("PlayerHurt");
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("enemyWeapon"))
            {
                _playerInfo.InputEnabled = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("enemyWeapon"))
            {
                _playerInfo.InputEnabled = true;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(wallCheckPoint.position, wallCheckSize);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(attackPoint.position, attackRange);
        }

        private void EnableInput()
        {
            _playerInfo.InputEnabled = true;
        }
        
        
    }
}
