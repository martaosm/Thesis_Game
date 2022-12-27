using System;
using NPC;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ChaptersControllers
{
    /**
     * Class checking player's statistics and then adapting environment to them in the scene 
     */
    public class PrologueController : MonoBehaviour
    {
        [SerializeField] private GameObject key;
        [SerializeField] private GameObject panel;
        private void OnEnable()
        {
            if (PlayerPrefs.GetString("spawnPoint").Equals("left"))
            {
                panel.SetActive(true);
            }
            //if player has the key in their possession or lost it, then key in the scene is not active
            if (PlayerPrefs.GetInt("hasKey") == 0 && PlayerPrefs.GetInt("IsFree") == 1 || PlayerPrefs.GetInt("hasKey") == 1 && PlayerPrefs.GetInt("IsFree") == 0)
            {
                key.SetActive(false);
            }
            
            //if player find mark object then skeletons are not attacking the player
            if (PlayerPrefs.GetInt("hasMark") == 1)
            {
                var skeletons = FindObjectsOfType<EnemyController>();
                foreach(var skeleton in skeletons)
                {
                    skeleton.gameObject.GetComponent<Collider2D>().isTrigger = true;
                }
            }

            //if condition is met, then key position is generated in this scene
            if (PlayerPrefs.GetInt("keyPosition") == 0 
                && PlayerPrefs.GetInt("hasKey") == 0 
                && !(PlayerPrefs.GetInt("hasKey") == 0 
                     && PlayerPrefs.GetInt("IsFree") == 1 
                     || PlayerPrefs.GetInt("hasKey") == 1 
                     && PlayerPrefs.GetInt("IsFree") == 0))
            {
                var x = Random.Range(-44, 2);
                var y = Random.Range(-21, 7);
                const int z = 0;
                var pos = new Vector3(x, y, z);
                key.transform.position = pos;
                key.SetActive(true);
            }
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.E))
            {
                panel.SetActive(false);
            }
        }
    }
}
