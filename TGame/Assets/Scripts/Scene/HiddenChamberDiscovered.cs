using System.Collections;
using Player;
using UnityEngine;

namespace Scene
{
    /**
     * Class controls behaviour when player discovers hidden chamber with mark object in it
     */
    public class HiddenChamberDiscovered : MonoBehaviour
    {
        private PlayerInfo _playerInfo;
        private Collider2D _collider2D;
        private bool _chamberDiscovered;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private Transform playerCheckPoint;
        [SerializeField] private Vector2 playerCheckSize;
    
        private void Start()
        {
            _playerInfo = FindObjectOfType<PlayerInfo>();
            _collider2D = GetComponent<Collider2D>();
        }
    
        private void Update()
        {
            //if player attacks wall, behind which is hidden chamber, coroutine start 
            if (IsBeingAttacked() && _playerInfo.IsAttacking && !_chamberDiscovered)
            {
                _collider2D.enabled = false;
                _chamberDiscovered = true;
                StartCoroutine(DoorDestroy());
            }
        }
    
        /**
         * checks if player is in range 
         */
        private bool IsBeingAttacked()
        {
            return Physics2D.OverlapBox(playerCheckPoint.position, playerCheckSize, 0, playerLayer);
        }
        
        /**
         * deactivates door and blackout that are covering hidden chamber
         */
        private IEnumerator DoorDestroy()
        {
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }
    }
}
