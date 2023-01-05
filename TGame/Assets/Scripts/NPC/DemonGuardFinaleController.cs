using Player;
using TMPro;
using UnityEngine;

namespace NPC
{
    /**
     * Class controls guard npc in final scene if player was not attacked by guard
     */
    public class DemonGuardFinaleController : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI panelText;
        [SerializeField] private TextMeshProUGUI instructions;
        [SerializeField] private GameObject player;
    
        private void Update()
        {
            //npc faces the player all the time
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
            //text panel is activated
            if (col.GetComponent<PlayerInfo>())
            {
                panel.SetActive(true);
                instructions.gameObject.SetActive(true);
                panelText.text =
                    "Hi. I found that bastard hiding in here. I caught them in time, before they crossed to the next circle. I found this in their corpse. As I said I don't support everything that the management does so I'm giving you this key.";
                instructions.text = "Press E to take the key";
            }
        }

        /**
         * when player gives input, key is transferred to player
         */
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerInfo playerInfo))
            {
                if (Input.GetKey(KeyCode.E))
                {
                    playerInfo.HasKey = true;
                    instructions.gameObject.SetActive(false);
                    panelText.text = "Good luck! You will need in the next circle of Hell";
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            //text panel is deactivated
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
}
