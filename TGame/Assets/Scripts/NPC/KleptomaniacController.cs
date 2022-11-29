using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Scene;
using UnityEngine;

public class KleptomaniacController : MonoBehaviour
{
    /*public int _whichSide; //right - 1, left - 0
    public bool _isAtCenterPoint = true;*/
    private Vector2 _attackPoint;
    private Animator _animator;
    private Collider _collider;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector2 leftPoint;
    [SerializeField] private Vector2 rightPoint;
    [SerializeField] private Vector2 centerPoint;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (player.transform.position.x > gameObject.transform.position.x)
        {
            gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
        else if (player.transform.position.x < gameObject.transform.position.x)
        {
            gameObject.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }

        if (!_animator.GetBool("Attack") && player.transform.position.x != transform.position.x && player.transform.position.y>=9.34f)
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                new Vector2(player.transform.position.x, transform.position.y), 5 * Time.deltaTime);
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                _animator.SetTrigger("Walk");
            }
            
        }
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
        gameObject.tag = "enemyWeapon";
        _animator.SetBool("Attack", true);
        transform.position = Vector2.MoveTowards(transform.position, 
            new Vector2(player.transform.position.x, transform.position.y), 10 * Time.deltaTime);
    }

    private void IdleState()
    {
        _animator.SetBool("Attack", false);
    }

    private void AfterDeath()
    {
        gameObject.tag = "Key";
        _collider.isTrigger = false;
    }
}
