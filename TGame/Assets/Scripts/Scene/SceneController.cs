using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private bool InputEnabled { get; set; } = true;
    private bool _nextChamber;
    private float _dirX;
    private CameraController _cameraController;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject checkpoint;

    private void OnEnable()
    {
        InputEnabled = true;
    }

    private void OnDisable()
    {
        InputEnabled = false;
    }

    private void Start()
    {
        _cameraController = FindObjectOfType<CameraController>();
    }

    private void Update()
    {
        if (_nextChamber)
        {
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(10, player.GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<PlayerInfo>())
        {
            InputEnabled = false;
            _dirX = Input.GetAxisRaw("Horizontal");
            _cameraController.enabled = false;
            _nextChamber = true;
            StartCoroutine(MoveToNextChamber());
        }
    }

    IEnumerator MoveToNextChamber()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Chapter1");
    }
    public bool GetInputEnabled()
    {
        return InputEnabled;
    }
}
