using System;
using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace NPC
{
    public class EnemyController : MonoBehaviour
    {
        private float _life = 10f;
        public float Life
        {
            get => _life;
            set => _life = value;
        }
        private Animator _animator;
        private PlayerInfo _playerInfo;
        private Collider2D _collider2D;
        private bool _isDefeated;
        [SerializeField] private Slider _slider;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject hitBox;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private Transform playerCheckPoint;
        [SerializeField] private float playerCheckSize;
        private static readonly int Attack = Animator.StringToHash("Attack");

        private void OnEnable()
        {
            PlayerInfo.OnGetMark += PlayerHasMark;
        }

        private void OnDisable()
        {
            PlayerInfo.OnGetMark -= PlayerHasMark;
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _playerInfo = FindObjectOfType<PlayerInfo>();
            _collider2D = GetComponent<Collider2D>();
            if (_playerInfo.HasMark)
            {
                _collider2D.isTrigger = true;
                hitBox.SetActive(false);
            }
        }
    
        private void Update()
        {
            if (_playerInfo.HasMark)
            {
                _collider2D.isTrigger = true;
                hitBox.SetActive(false);
            }
            
            if (_life <= 0)
            {
                _slider.gameObject.SetActive(false);
                hitBox.SetActive(false);
                playerCheckPoint.gameObject.SetActive(false);
                //_collider2D.enabled = false;
            }
            else
            {
                _slider.value = _life;
            }
            
            if (!_playerInfo.HasMark && _playerInfo._health > 0)
            {
                if (Physics2D.OverlapCircleAll(playerCheckPoint.position, playerCheckSize, playerLayer).Length > 0 &&
                    _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && !_isDefeated)
                {
                    _animator.SetBool(Attack, true);
                }
                else
                {
                    _animator.SetBool(Attack, false);
                } 
            }

            if (_life > 0)
            {
                if (player.transform.position.x > gameObject.transform.position.x)
                {
                    gameObject.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
                }
                else if (player.transform.position.x < gameObject.transform.position.x)
                {
                    gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                } 
            }
        }

        private void PlayerHasMark()
        {
            _collider2D.isTrigger = true;
            hitBox.SetActive(false);
        }
        
        public IEnumerator DeadBodyDestroy()
        {
            _isDefeated = true;
            yield return new WaitForSeconds(1f);
            var o = gameObject;
            o.layer = 0;
            o.tag = "Life";
            _collider2D.isTrigger = true;
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color=Color.blue;
            Gizmos.DrawSphere(playerCheckPoint.position, playerCheckSize);
        }
    }
}
