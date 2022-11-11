using System;
using System.Collections;
using Player;
using UnityEngine;

namespace NPC.Enemies
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
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private Transform playerCheckPoint;
        [SerializeField] private float playerCheckSize;
        private static readonly int Attack = Animator.StringToHash("Attack");

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _playerInfo = FindObjectOfType<PlayerInfo>();
            _collider2D = GetComponent<Collider2D>();
        }
    
        private void Update()
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
        
        public IEnumerator DeadBodyDestroy()
        {
            _isDefeated = true;
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color=Color.blue;
            Gizmos.DrawSphere(playerCheckPoint.position, playerCheckSize);
        }
    }
}
