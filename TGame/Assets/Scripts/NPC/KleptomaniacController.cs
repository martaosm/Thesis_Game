using System;
using System.Collections;
using Player;
using Scene;
using UnityEngine;
using UnityEngine.UI;

namespace NPC
{
    /**
     * Class controls fire npc behaviour in the scene "Finale"
     */
    public class KleptomaniacController : MonoBehaviour
    {
        private float _health = 25f;
        public float Health
        {
            get => _health;
            set => _health = value;
        }
        private bool _isDead;
        private Animator _animator;
        private Collider2D _collider;
        private Rigidbody2D _rb;
        [SerializeField] private Vector2 center;
        [SerializeField] private Slider slider;
        [SerializeField] private GameObject player;
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Walk = Animator.StringToHash("Walk");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider2D>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            //health bar control
            if (_health <= 0)
            {
                slider.gameObject.SetActive(false);
            }
            else
            {
                slider.value = _health;
            }
            
            if (!_isDead)
            {
                //condition to face a player
                if (player.transform.position.x > gameObject.transform.position.x)
                {
                    gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                }
                else if (player.transform.position.x < gameObject.transform.position.x)
                {
                    gameObject.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
                }

                //when player is above certain height npc walks instead of being in attack mode
                if (!_animator.GetBool(Attack) &&
                    Math.Abs(player.transform.position.x - transform.position.x) > 1.5f &&
                    player.transform.position.y >= 9.34f)
                {
                    var position = transform.position;
                    position = Vector2.MoveTowards(position,
                        new Vector2(player.transform.position.x, position.y), 5 * Time.deltaTime);
                    transform.position = position;
                    if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                        _animator.SetTrigger(Walk);
                    }
                }  
            }
        }
    
        private void OnEnable()
        {
            TriggerAreaController.OnAttack += FireAttack;
            TriggerAreaController.OnIdle += IdleState;
            TriggerAreaController.OnBackToCenter += BackToCenter;
            PlayerMovement.OnKleptoDeath += AfterDeath;
        }

        private void OnDisable()
        {
            TriggerAreaController.OnAttack -= FireAttack;
            TriggerAreaController.OnIdle -= IdleState;
            TriggerAreaController.OnBackToCenter -= BackToCenter;
            PlayerMovement.OnKleptoDeath -= AfterDeath;
        }
    
        /**
         * npc is in attack mode and hits player
         */
        private void FireAttack()
        {
            if (!_isDead)
            {
                gameObject.tag = "enemyWeapon";
                _animator.SetBool(Attack, true);
                var position = transform.position;
                position = Vector2.MoveTowards(position,
                    new Vector2(player.transform.position.x, position.y), 10 * Time.deltaTime);
                transform.position = position;
            }
        }
        
        /**
         * sets animation to "Walk"
         */
        private void IdleState()
        {
            _animator.SetBool(Attack, false);
        }

        /**
         * sets animation to "Walk"
         */
        private void BackToCenter()
        {
            _animator.SetBool(Attack, false);
            var position = transform.position;
            position = Vector2.MoveTowards(position,
                center, 10 * Time.deltaTime);
            transform.position = position;
        }

        /**
         * when player defeats npc, this method turns it into key
         */
        private void AfterDeath()
        {
            gameObject.tag = "Key";
            _collider.isTrigger = true;
            _isDead = true;
            _rb.bodyType = RigidbodyType2D.Static;
            StartCoroutine(ChangeSprite());
        }

        /**
         * changes npc into a key sprite
         */
        private IEnumerator ChangeSprite()
        {
            yield return new WaitForSeconds(1f);
            _animator.Play("KeyAnimationKlepto");
        }

        /**
         * when npc hits player, npc bounces off of player with force
         */
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.GetComponent<PlayerInfo>())
            {
                _rb.AddForce(col.contacts[0].normal * 600f);
            }
        }
    }
}
