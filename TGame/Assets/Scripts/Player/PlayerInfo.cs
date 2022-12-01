using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Player
{
    public class PlayerInfo : MonoBehaviour
    {
        public float _health = 100;
        private bool _isAttacking;
        public bool _hasKey;
        public bool _hasMark;
        private bool _inputEnabled = true;
        [SerializeField] private Slider _slider;
        public bool InputEnabled
        {
            get => _inputEnabled;
            set => _inputEnabled = value;

        }
        public bool HasKey
        {
            get => _hasKey;
            set => _hasKey = value;
        }

        public bool HasMark
        {
            get => _hasMark;
            set => _hasMark = value;
        }

        private void Update()
        {
            if (_health <= 0)
            {
                _slider.gameObject.SetActive(false);
            }
            else
            {
                _slider.value = _health;
            }
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
            
            if (col.gameObject.CompareTag("Mark"))
            {
                _hasMark = true;
                col.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            _hasKey = PlayerPrefs.GetInt("hasKey") == 1;
            _hasMark = PlayerPrefs.GetInt("hasMark") == 1;
            _health = PlayerPrefs.GetInt("PlayerHealth", (int)_health);
        }

        private void OnDisable()
        {
            PlayerPrefs.SetInt("hasKey", _hasKey ? 1 : 0);
            PlayerPrefs.SetInt("hasMark", _hasMark ? 1 : 0);
            PlayerPrefs.SetInt("PlayerHealth", (int)_health);
        }
        
    }
}
