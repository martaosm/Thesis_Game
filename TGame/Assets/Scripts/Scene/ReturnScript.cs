using System.Collections;
using System.Collections.Generic;
using Player;
using Scene;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnScript : MonoBehaviour
{
    private bool _nextChamber;
    private bool _dirX;
    private CameraController _cameraController;
    private SceneController _sceneController;
    [SerializeField] private GameObject player;
    
    private void OnEnable()
    {
        player.GetComponent<PlayerInfo>().InputEnabled = false;
        _cameraController = FindObjectOfType<CameraController>();
        //_cameraController.enabled = false;
        _nextChamber = true;
    }
    
    private void Update()
    {
        var dir = 0;
        _dirX = player.GetComponent<SpriteRenderer>().flipX;
        if (_dirX == true)
        {
            dir = -1;
        }
        else if (_dirX == false)
        {
            dir = 1;
        }
        if (_nextChamber)
        {
            player.GetComponent<Rigidbody2D>().velocity =
                new Vector2(dir * 10, player.GetComponent<Rigidbody2D>().velocity.y);
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<PlayerInfo>())
        {
            if (_dirX == true)
            {
                player.GetComponent<PlayerInfo>().InputEnabled = false;
                _cameraController.enabled = false;
                _nextChamber = true;
                StartCoroutine(MoveToNextChamber());
            }
            else if (_dirX == false)
            {
                player.GetComponent<PlayerInfo>().InputEnabled = true;
                _cameraController.enabled = true;
                _nextChamber = false;
            }
        }
    }
    
    IEnumerator MoveToNextChamber()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
