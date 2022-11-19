using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class KleptomaniacController : MonoBehaviour
{
    public int _whichSide; //right - 1, left - 0
    public bool _isAtCenterPoint = true;
    private Vector2 _attackPoint;
    private Animator _animator;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector2 leftPoint;
    [SerializeField] private Vector2 rightPoint;
    [SerializeField] private Vector2 centerPoint;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player.transform.position.x > gameObject.transform.position.x)
        {
            gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            //_attackPoint = rightPoint;
        }
        else if (player.transform.position.x < gameObject.transform.position.x)
        {
            gameObject.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
            //_attackPoint = leftPoint;
        }

        //_isAtCenterPoint = transform.position.x == centerPoint.x;
    }
    
    private void OnEnable()
    {
        TriggerAreaController.OnAttack += FireAttack;
        TriggerAreaController.OnIdle += IdleState;
    }

    private void OnDisable()
    {
        TriggerAreaController.OnAttack -= FireAttack;
        TriggerAreaController.OnIdle -= IdleState;
    }
    
    private void FireAttack()
    {
        _attackPoint = _whichSide == 1 ? rightPoint : leftPoint;
        Debug.Log(_attackPoint);
        gameObject.tag = "enemyWeapon";
        _animator.SetBool("Attack", true);
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x, transform.position.y), 15 * Time.deltaTime);
        //yield return new WaitForSeconds(1f);
        //_attackPoint = centerPoint;
        //transform.position = Vector2.MoveTowards(transform.position, _attackPoint.position, 7 * Time.deltaTime);
    }

    private void IdleState()
    {
        _animator.SetBool("Attack", false);
    }
}
