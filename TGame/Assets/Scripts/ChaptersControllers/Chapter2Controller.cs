using NPC;
using UnityEngine;

namespace ChaptersControllers
{
    /**
     * Class checking player's statistics and then adapting environment to them in the scene 
     */
    public class Chapter2Controller : MonoBehaviour
    {
        public bool demonGuardDone;
        [SerializeField] private GameObject hiddenChamber;
        [SerializeField] private GameObject mark;
        [SerializeField] private GameObject demonGuard;
        [SerializeField] private GameObject key;
        private void OnEnable()
        {
            //if player has the key in their possession or lost it, then key in the scene is not active
            if (PlayerPrefs.GetInt("hasKey") == 0 && PlayerPrefs.GetInt("IsFree") == 1 || PlayerPrefs.GetInt("hasKey") == 1 && PlayerPrefs.GetInt("IsFree") == 0)
            {
                key.SetActive(false);
            }
            
            //if player set prisoner free then they meet guard Npc in this scene
            if (PlayerPrefs.GetInt("IsFree") == 1 && PlayerPrefs.GetInt("DemonGuardDone") == 0)
            {
                demonGuard.SetActive(true);
            }
            
            //if player find mark object then skeletons are not attacking the player and chamber in which mark was found is left open
            if (PlayerPrefs.GetInt("hasMark") == 1)
            {
                mark.SetActive(false);
                hiddenChamber.SetActive(false);
                var skeletons = FindObjectsOfType<EnemyController>();
                foreach(var skeleton in skeletons)
                {
                    skeleton.gameObject.GetComponent<Collider2D>().isTrigger = true;
                }
            }
            
            //if condition is met, then key position is generated in this scene
            if (PlayerPrefs.GetInt("keyPosition") == 1 
                && PlayerPrefs.GetInt("hasKey") == 0
                && !(PlayerPrefs.GetInt("hasKey") == 0 
                     && PlayerPrefs.GetInt("IsFree") == 1 
                     || PlayerPrefs.GetInt("hasKey") == 1 
                     && PlayerPrefs.GetInt("IsFree") == 0))
            {
                var random = Random.Range(0, 1);
                var x = 0;
                var y = 0;
                switch (random)
                {
                    case 1:
                        x = Random.Range(41, 96);
                        y = Random.Range(-14, 15);
                        break;
                    case 0:
                        x = Random.Range(-24, -10);
                        y = Random.Range(4, 16);
                        break;
                }
                const int z = 0;
                var pos = new Vector3(x, y, z);
                key.transform.position = pos;
                key.SetActive(true);
            }
        }

        private void OnDisable()
        {
            PlayerPrefs.SetInt("DemonGuardDone", demonGuardDone ? 1 : 0);
        }
    }
}
