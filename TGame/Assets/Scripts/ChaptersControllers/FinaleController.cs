using UnityEngine;

namespace ChaptersControllers
{
    /**
     * Class checking player's statistics and then adapting environment to them in the scene 
     */
    public class FinaleController : MonoBehaviour
    {
        private bool _demonGuardDone;
        private bool _isGuardAnEnemy;
        private bool _isFree;
        [SerializeField] private GameObject kleptoNpc;
        [SerializeField] private GameObject demonGuard;
        [SerializeField] private GameObject key;
        
        /**
         * method in which PlayerPrefs are saved to variables and checking:
         * - if player finished interaction with guard and did not fought them, then guard is active in this scene
         * - if player didn't finish interaction with guard or finished it, but fought with the guard then fire npc is active in this scene
         * - if player has the key in their possession or lost it, then key in the scene is not active
         * - if condition is met, then key position is generated in this scene
         */
        private void OnEnable()
        {
            _demonGuardDone = PlayerPrefs.GetInt("DemonGuardDone") == 1;
            _isGuardAnEnemy = PlayerPrefs.GetInt("IsGuardAnEnemy") == 1;
            _isFree = PlayerPrefs.GetInt("IsFree") == 1;
            
            if (_demonGuardDone && !_isGuardAnEnemy)
            {
                demonGuard.SetActive(true);
            }
            
            if (_isFree && _isGuardAnEnemy || _isFree && !_demonGuardDone)
            {
                kleptoNpc.SetActive(true);
            }
            
            if (PlayerPrefs.GetInt("hasKey") == 0 && PlayerPrefs.GetInt("IsFree") == 1 || PlayerPrefs.GetInt("hasKey") == 1 && PlayerPrefs.GetInt("IsFree") == 0)
            {
                key.SetActive(false);
                
            }
            
            if (PlayerPrefs.GetInt("keyPosition") == 2 && PlayerPrefs.GetInt("hasKey") == 0 
                                                       && !(PlayerPrefs.GetInt("hasKey") == 0 && PlayerPrefs.GetInt("IsFree") == 1 
                                                            || PlayerPrefs.GetInt("hasKey") == 1 && PlayerPrefs.GetInt("IsFree") == 0))
            {
                var x = Random.Range(56, 103);
                var y = Random.Range(10, 34);
                const int z = 0;
                var pos = new Vector3(x, y, z);
                key.transform.position = pos;
                key.SetActive(true);
            }
        }
    }
}
