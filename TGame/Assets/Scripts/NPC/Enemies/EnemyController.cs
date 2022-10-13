using System.Collections;
using Player;
using UnityEngine;

namespace NPC.Enemies
{
    public class EnemyController : MonoBehaviour
    {
        private float _life;
        private Animator _animator;
        private PlayerInfo _playerInfo;
        private static readonly int Death = Animator.StringToHash("Death");
        private Collider2D _collider2D;
        private bool _isDefeated;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private Transform playerCheckPoint;
        [SerializeField] private Vector2 playerCheckSize;
    
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _playerInfo = FindObjectOfType<PlayerInfo>();
            _collider2D = GetComponent<Collider2D>();
        }
    
        private void Update()
        {
            if (IsBeingAttacked() && _playerInfo.GetIsAttacking() && !_isDefeated)
            {
                _animator.SetTrigger(Death);
                _collider2D.enabled = false;
                _isDefeated = true;
                StartCoroutine(DeadBodyDestroy());
            }
        }
    
        private bool IsBeingAttacked()
        {
            return Physics2D.OverlapBox(playerCheckPoint.position, playerCheckSize, 0, playerLayer);
        }
        
        
        IEnumerator DeadBodyDestroy()
        {
            yield return new WaitForSeconds(2f);
            this.gameObject.SetActive(false);
        }
    
    }
}
