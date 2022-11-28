using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;

public class DemonGuardFinaleController : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI panelText;
    [SerializeField] private TextMeshProUGUI instructions;
    [SerializeField] private GameObject player;
    
    private void Update()
    {
        if (player.transform.position.x > gameObject.transform.position.x)
        {
            gameObject.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        }
        else if (player.transform.position.x < gameObject.transform.position.x)
        {
            gameObject.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<PlayerInfo>())
        {
            panel.SetActive(true);
            instructions.gameObject.SetActive(true);
            panelText.text =
                "Hi. I found that bastard hiding in here. I caught them in time, before they crossed to the next circle. I found this in their corpse. As I said I don't support everything that the management does so I'm giving you this key.";
            instructions.text = "Press E to take the key";
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerInfo playerInfo))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                playerInfo.HasKey = true;
                instructions.gameObject.SetActive(false);
                panelText.text = "Good luck! You will need in the next circle of Hell";
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerInfo>())
        {
            panel.SetActive(false);
            instructions.gameObject.SetActive(false);
        }
        
        if (other.TryGetComponent(out PlayerInfo playerInfo) && playerInfo.HasKey)
        {
            panel.SetActive(false);
            instructions.gameObject.SetActive(false);
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }
}
