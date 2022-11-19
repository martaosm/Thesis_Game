using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

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
        if (IsBeingAttacked() && _playerInfo.GetIsAttacking() && !_chamberDiscovered)
        {
            _collider2D.enabled = false;
            _chamberDiscovered = true;
            StartCoroutine(DoorDestroy());
        }
    }
    
    private bool IsBeingAttacked()
    {
        return Physics2D.OverlapBox(playerCheckPoint.position, playerCheckSize, 0, playerLayer);
    }
        
        
    IEnumerator DoorDestroy()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
