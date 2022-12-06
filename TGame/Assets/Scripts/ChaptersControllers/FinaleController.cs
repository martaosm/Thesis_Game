using UnityEngine;

namespace ChaptersControllers
{
    public class FinaleController : MonoBehaviour
    {
        //private bool _isDoorOpened;
        private bool _demonGuardDone;
        private bool _isGuardAnEnemy;
        private bool _isFree;
        [SerializeField] private GameObject kleptoNpc;
        [SerializeField] private GameObject demonGuard;
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
            
        }
    }
}
