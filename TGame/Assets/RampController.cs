using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class RampController : MonoBehaviour
{
    [SerializeField] private GameObject point1;
    [SerializeField] private GameObject point2;
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerInfo>())
        {
            collision.gameObject.transform.SetParent(transform);
            transform.position = Vector2.MoveTowards(transform.position, point1.transform.position, Time.deltaTime * 10);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PlayerInfo>())
        {
            other.gameObject.transform.SetParent(null);
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<PlayerInfo>())
        {
            transform.position = Vector2.MoveTowards(transform.position, point2.transform.position, Time.deltaTime * 10);
        }
    }
}
