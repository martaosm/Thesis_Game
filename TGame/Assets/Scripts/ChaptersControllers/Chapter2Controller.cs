using NPC;
using UnityEngine;

namespace ChaptersControllers
{
    public class Chapter2Controller : MonoBehaviour
    {
        public bool demonGuardDone;
        [SerializeField] private GameObject hiddenChamber;
        [SerializeField] private GameObject mark;
        [SerializeField] private GameObject demonGuard;
        private void OnEnable()
        {
            if (PlayerPrefs.GetInt("IsFree") == 1 && PlayerPrefs.GetInt("DemonGuardDone") == 0)
            {
                demonGuard.SetActive(true);
            }

            if (PlayerPrefs.GetInt("hasMark") == 1)
            {
                mark.SetActive(false);
                hiddenChamber.SetActive(false);
            }
        
            if (PlayerPrefs.GetInt("hasMark") == 1)
            {
                var skeletons = FindObjectsOfType<EnemyController>();
                foreach(var skeleton in skeletons)
                {
                    skeleton.gameObject.GetComponent<Collider2D>().isTrigger = true;
                }
            }
        }

        private void OnDisable()
        {
            PlayerPrefs.SetInt("DemonGuardDone", demonGuardDone ? 1 : 0);
        }
    }
}
