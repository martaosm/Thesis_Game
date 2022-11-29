using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class BlackoutController : MonoBehaviour
{
    //private int _currentIndex = 0;
    [SerializeField] private GameObject ramp;
    [SerializeField] private GameObject blackout;
    [SerializeField] private GameObject point2;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<PlayerInfo>())
        {
            Destroy(blackout);
            /*if (Vector2.Distance(waypoints[_currentIndex].transform.position, transform.position) < .1f)
            {
                _currentIndex++;
                if (_currentIndex >= waypoints.Length)
                {
                    _currentIndex = 0;
                }
            }*/
            ramp.transform.position = Vector2.MoveTowards(ramp.transform.position, point2.transform.position, Time.deltaTime * 2);
        }
    }
}
