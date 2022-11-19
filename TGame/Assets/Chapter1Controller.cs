using System;
using System.Collections;
using System.Collections.Generic;
using NPC;
using Scene;
using UnityEngine;

public class Chapter1Controller : MonoBehaviour
{
    private bool _isFireFree;
    [SerializeField] private GameObject cellDoor;
    [SerializeField] private GameObject cellWall;
    [SerializeField] private float speed;
    private void OnEnable()
    {
        _isFireFree = PlayerPrefs.GetInt("IsFree") == 1 ? true : false;
        if (_isFireFree)
        {
            FindObjectOfType<FireNpcController>().gameObject.SetActive(false);
            cellDoor.GetComponent<Animator>().SetBool("CellDoorOpened", true);//transform.position = Vector2.MoveTowards(transform.position, cellWall.transform.position, speed);
            FindObjectOfType<CellDoorController>().gameObject.GetComponent<Collider2D>().enabled = false;
            cellDoor.GetComponent<Collider2D>().enabled = false;
        }
    }
    
}
