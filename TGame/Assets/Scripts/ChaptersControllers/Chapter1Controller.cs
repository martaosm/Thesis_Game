using NPC;
using Scene;
using UnityEngine;

namespace ChaptersControllers
{
    /**
     * Class checking player's statistics and then adapting environment to them in the scene 
     */
    public class Chapter1Controller : MonoBehaviour
    {
        private bool _isFireFree;
        [SerializeField] private GameObject cellDoor;
        private static readonly int CellDoorOpened = Animator.StringToHash("CellDoorOpened");
        
        /**
         * method checking on enable if player let FireNpc free then cell door have to be opened all the time and
         * if player find mark object then skeletons are not attacking the player
         */
        private void OnEnable()
        {
            
            _isFireFree = PlayerPrefs.GetInt("IsFree") == 1;
            if (_isFireFree)
            {
                FindObjectOfType<FireNpcController>().gameObject.SetActive(false);
                cellDoor.GetComponent<Animator>().SetBool(CellDoorOpened, true);
                FindObjectOfType<CellDoorController>().gameObject.GetComponent<Collider2D>().enabled = false;
                cellDoor.GetComponent<Collider2D>().enabled = false;
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
    }
}
