using System.Collections;
using NPC;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("For WallSliding")] 
        private bool _isTouchingWall;
        private bool _isWallSliding;
        [SerializeField] private float wallSlideSpeed;
        [SerializeField] private LayerMask wallLayer;
        [SerializeField] private Transform wallCheckPoint;
        [SerializeField] private Vector2 wallCheckSize;
        
        private float _dirX;
        private bool _isBeingAttacked;
        private bool _isPlayerCrouching;
        private PlayerInfo _playerInfo;
        private Rigidbody2D _rb;
        private SpriteRenderer _player;
        private Animator _animation;
        private BoxCollider2D _collider;
        
        public float DirX
        {
            get => _dirX;
            set => _dirX = value;
        }
        
        private MovementState _state;
        private bool _canMove = true;
        private const float AttackRate = 2f;
        private float _nextAttackTime;
        [SerializeField] private float jumpForce = 17f;
        [SerializeField] private float speed = 10f;
        [SerializeField] private LayerMask jumpGround;
        [SerializeField] private Transform attackPoint;
        [SerializeField] private float attackRange;
        [SerializeField] private LayerMask enemyLayer;
        public delegate void KleptoDeath();
        public static event KleptoDeath OnKleptoDeath;
        private static readonly int State = Animator.StringToHash("state");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int Death = Animator.StringToHash("Death");
        private static readonly int HurtFromFire = Animator.StringToHash("HurtFromFire");
        private static readonly int Crouch = Animator.StringToHash("Crouch");
        private static readonly int Hurt = Animator.StringToHash("Hurt");

        //enum for movement state, determines what motion player is currently doing
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
        }

        private void Update()
        {
            //if player can move and input is enabled
            if (_canMove && _playerInfo.InputEnabled)
            {
                //if player is crouching they can move from side to side
                if (_isPlayerCrouching)
                {
                    _rb.velocity = new Vector2(0, 0);
                }
                
                //moving from side to side and jumping
                if (!_isPlayerCrouching)
                {
                    _dirX = Input.GetAxisRaw("Horizontal");
                    _rb.velocity = new Vector2(_dirX * speed, _rb.velocity.y);
                    //jump is available only if player is grounded
                    if (Input.GetButtonDown("Jump") && IsGrounded() && _playerInfo.InputEnabled)
                    {
                        _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
                    }
                }
            }
            
            UpdateAnimation();
            PlayerAttack(IsGrounded());
            WallSlide();

            //player's "death" sequence, animation is played and coroutine started
            if (_playerInfo.Health <= 0)
            {
                _animation.Play("PlayerDeath");
                StartCoroutine(ChangeToGameOverScene());
                _playerInfo.Health = 1;
            }
        }

        //depends on enum, method changes animation, method also flips player's sprite depending on Horizontal input
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
            
            //if velocity.y is positive the player is in state of jumping, if it's negative then in state of falling
            if (_rb.velocity.y > .1f)
            {
                _state = MovementState.Jump;
            }
            else if (_rb.velocity.y < -.1f)
            {
                _state = MovementState.Fall;
            }

            //if player is touching wall and is not grounded then they slide down using the wall
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

            //using C key player can crouch
            if (Input.GetKey(KeyCode.C))
            {
                _animation.SetBool(Crouch, true);
                _state = MovementState.Crouch;
                _isPlayerCrouching = true;
            }

            //if C key is up player stops crouching
            if (Input.GetKeyUp(KeyCode.C))
            {
                _animation.SetBool(Crouch, false);
                _isPlayerCrouching = false;
            }

            _animation.SetInteger(State, (int)_state);
        }
        
        //attack sequence
        private void PlayerAttack(bool grounded)
        {
            //condition which makes player play one attack animation at the time, so spamming the mouse button won't make player attack even after stopping
            if (Time.time >= _nextAttackTime)
            {
                if (!_isPlayerCrouching)
                {
                    //if mouse button is pressed and player is grounded attack sequence starts
                    if (Input.GetKeyDown(KeyCode.Mouse0) && grounded && _playerInfo.InputEnabled)
                    {
                        StartCoroutine(PlayerExecuteAttack());
                        _rb.velocity = new Vector2(0, _rb.velocity.y);
                        _canMove = false;
                        _playerInfo.IsAttacking = true;
                        _nextAttackTime = Time.time + 1f / AttackRate;
                    }
                }
            }
        }

        //attack execution
        private IEnumerator PlayerExecuteAttack()
        {
            _animation.SetTrigger(Attack);
            //checks if enemy is in range of the attack
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
            foreach (var hitEnemy in hitEnemies)
            {
                //sequence for attacking skeletons
                if (hitEnemy.TryGetComponent(out EnemyController enemyController))
                {
                    //taking skeleton hp if hit
                    enemyController.Life -= 2;
                    if (enemyController.Life <= 0)
                    {
                        //player defeats skeleton
                        hitEnemy.gameObject.GetComponent<Animator>().SetTrigger(Death);
                        StartCoroutine(hitEnemy.gameObject.GetComponent<EnemyController>().DeadBodyDestroy());
                        break;
                    }
                    hitEnemy.gameObject.GetComponent<Animator>().SetTrigger(Hit);
                }

                //sequence for attacking demon guard
                if (hitEnemy.TryGetComponent(out DemonGuardController demonGuardController))
                {
                    //taking guard hp if hit
                    demonGuardController.Life -= 5;
                    if (demonGuardController.Life <= 10)
                    {
                        break;
                    }
                    StartCoroutine(ChangeEnemyColorWhenHit(hitEnemy.gameObject));
                }
                
                //sequence for attacking candy npc
                if (hitEnemy.TryGetComponent(out CandyGiverController candyGiverController))
                {
                    //taking npc hp if hit
                    candyGiverController.Life -= 5;
                    if (candyGiverController.Life <= 0)
                    {
                        break;
                    }

                    GameObject o;
                    (o = hitEnemy.gameObject).GetComponent<Animator>().SetTrigger(Hurt);
                    StartCoroutine(ChangeEnemyColorWhenHit(o));
                }

                //sequence for attacking fire npc
                if (hitEnemy.TryGetComponent(out KleptomaniacController kleptomaniacController))
                {
                    //taking npc hp if hit
                    kleptomaniacController.Health -= 5f;
                    if (kleptomaniacController.Health <= 0)
                    {
                        hitEnemy.gameObject.GetComponent<Animator>().SetTrigger(Death);
                        OnKleptoDeath?.Invoke();
                    }
                    StartCoroutine(ChangeEnemyColorWhenHit(hitEnemy.gameObject));
                }
            }
            yield return new WaitForSeconds(1f);
            //when attack ends player can move again
            _canMove = true;
            _playerInfo.IsAttacking = false;
        }

        //method controls wall slide action
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

        //checks if player is grounded
        private bool IsGrounded()
        {
            var bounds = _collider.bounds;
            return Physics2D.BoxCast(bounds.center, bounds.size, 0f, Vector2.down, .1f, jumpGround);
        }
        
        //checks if player is touching the wall
        private bool IsTouchingWall()
        {
            _isTouchingWall = Physics2D.OverlapBox(wallCheckPoint.position, wallCheckSize, 0, wallLayer);
            return _isTouchingWall;
        }

        //changes scene to game over scene
        private IEnumerator ChangeToGameOverScene()
        {
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene("GameOverScene");
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            //player takes damage if is hit by enemy, input is disabled
            if (col.gameObject.CompareTag("enemyWeapon"))
            {
                _playerInfo.InputEnabled = false;
                _playerInfo.Health -= 10;
                _animation.Play("PlayerHurt");
                
            }
        }

        //player takes damage if is hit by enemy, input is disabled
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.CompareTag("enemyWeapon"))
            {
                _playerInfo.InputEnabled = false;
                _playerInfo.Health -= 10;//_health
                _animation.Play("PlayerHurt");
            }
        }

        //after taking a hit input is enabled again
        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("enemyWeapon"))
            {
                _playerInfo.InputEnabled = true;
            }
        }

        //after taking a hit input is enabled again
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("enemyWeapon"))
            {
                _playerInfo.InputEnabled = true;
            }
        }

        //draws shapes on player, help for programmer
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(wallCheckPoint.position, wallCheckSize);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(attackPoint.position, attackRange);
        }

        //changes enemy color for some period if enemy is hit by player
        private IEnumerator ChangeEnemyColorWhenHit(GameObject enemy)
        {
            enemy.GetComponent<SpriteRenderer>().color = new Color(29f, 0f, 0f, 255); 
            yield return new WaitForSeconds(0.5f);
            enemy.GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f, 255); 
        }
    }
}
