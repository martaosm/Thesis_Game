using System;
using UnityEngine;

namespace Player
{
    public class PlayerInfo : MonoBehaviour
    {
        private bool _isAttacking;
        private bool _hasKey;
        public bool HasKey
        {
            get => _hasKey;
            set => _hasKey = value;
        }

        public bool GetIsAttacking()
        {
            return _isAttacking;
        }

        public void SetIsAttacking(bool isAttacking)
        {
            _isAttacking = isAttacking;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Key"))
            {
                _hasKey = true;
                col.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            _hasKey = PlayerPrefs.GetInt("hasKey") == 1;
        }

        private void OnDisable()
        {
            PlayerPrefs.SetInt("hasKey", _hasKey ? 1 : 0);
        }
        
    }
}
