using System;
using System.Collections;
using Player;
using Scene;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace NPC
{
    public class KleptomaniacController : MonoBehaviour
    {
        /*public int _whichSide; //right - 1, left - 0
    public bool _isAtCenterPoint = true;*/
        public float _health = 25f;
        private Vector2 _attackPoint;
        private Animator _animator;
        private Collider2D _collider;
        private Rigidbody2D _rb;
        private bool _isDead;
        [SerializeField] private Slider _slider;
        [SerializeField] private Sprite _newSprite;
        [SerializeField] private GameObject player;
        [SerializeField] private Vector2 leftPoint;
        [SerializeField] private Vector2 rightPoint;
        [SerializeField] private Vector2 centerPoint;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider2D>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (_health <= 0)
            {
                _slider.gameObject.SetActive(false);
            }
            else
            {
                _slider.value = _health;
            }
            
            if (!_isDead)
            {
                if (player.transform.position.x > gameObject.transform.position.x)
                {
                    gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                }
                else if (player.transform.position.x < gameObject.transform.position.x)
                {
                    gameObject.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
                }

                if (!_animator.GetBool("Attack") &&
                    Math.Abs(player.transform.position.x - transform.position.x) > 1.5f &&
                    player.transform.position.y >= 9.34f)
                {
                    transform.position = Vector2.MoveTowards(transform.position,
                        new Vector2(player.transform.position.x, transform.position.y), 5 * Time.deltaTime);
                    if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                        _animator.SetTrigger("Walk");
                    }
                }  
            }
        }
    
        private void OnEnable()
        {
            TriggerAreaController.OnAttack += FireAttack;
            TriggerAreaController.OnIdle += IdleState;
            PlayerMovement.OnKleptoDeath += AfterDeath;
        }

        private void OnDisable()
        {
            TriggerAreaController.OnAttack -= FireAttack;
            TriggerAreaController.OnIdle -= IdleState;
            PlayerMovement.OnKleptoDeath -= AfterDeath;
        }
    
        private void FireAttack()
        {
            if (!_isDead)
            {
                gameObject.tag = "enemyWeapon";
                _animator.SetBool("Attack", true);
                //if (Math.Abs(player.transform.position.x - transform.position.x) > 1.25f)
                //{
                transform.position = Vector2.MoveTowards(transform.position,
                    new Vector2(player.transform.position.x, transform.position.y), 10 * Time.deltaTime);
                //} 
            }
            
        
        }

        private void IdleState()
        {
            _animator.SetBool("Attack", false);
        }

        private void AfterDeath()
        {
            gameObject.tag = "Key";
            _collider.isTrigger = true;
            _isDead = true;
            _rb.bodyType = RigidbodyType2D.Static;
            StartCoroutine(ChangeSprite());
        }

        IEnumerator ChangeSprite()
        {
            yield return new WaitForSeconds(1f);
            _animator.Play("KeyAnimationKlepto");
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.GetComponent<PlayerInfo>())
            {
                _rb.AddForce(col.contacts[0].normal * 600f);
            }
        }
    }
}
