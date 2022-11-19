using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class TriggerAreaController : MonoBehaviour
{
    private KleptomaniacController _kleptoNpcController;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject kleptoNpc;
    public delegate void Attack();
    public static event Attack OnAttack;
    public delegate void Idle();
    public static event Idle OnIdle;

    private void Start()
    {
        _kleptoNpcController = kleptoNpc.GetComponent<KleptomaniacController>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (player.transform.position.x > kleptoNpc.transform.position.x)
        {
            _kleptoNpcController._whichSide = 1;
            //_attackPoint = rightPoint;
        }
        else if (player.transform.position.x < kleptoNpc.transform.position.x)
        {
            _kleptoNpcController._whichSide = 0;
            //_attackPoint = leftPoint;
        }
        /*if (col.GetComponent<PlayerInfo>())
        {
            OnAttack?.Invoke();
        }*/
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<PlayerInfo>() && other.transform.position.y>=9.338f && other.transform.position.y<=9.34f)
        {
            OnAttack?.Invoke();
        }
        if (other.GetComponent<PlayerInfo>() && other.transform.position.y>=9.34f)
        {
            OnIdle?.Invoke();
        }
    }
}
