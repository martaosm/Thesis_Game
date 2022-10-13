using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSwayer : MonoBehaviour
{
    private Vector2 _positionStart;

    [SerializeField] private float moveSpeed = 1.0f;
    private Vector2 _positionChange;
    
    private void Start()
    {
        _positionStart = transform.position;
        
    }
    
    private void Update()
    {
        _positionChange = transform.position;
        _positionChange.y = _positionStart.y + (moveSpeed * Mathf.Sin(Time.time));
        transform.position = _positionChange;
        
    }
}
