using System;
using UnityEngine;
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
        private String _spawnPoint;
        private PlayerMovement _playerMovement;
        [SerializeField] private Slider slider;
        [SerializeField] private Transform rightSpawnPoint;
        [SerializeField] private Transform leftSpawnPoint;
        public delegate void GetMark();
        public static event GetMark OnGetMark;
        
        
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
                slider.gameObject.SetActive(false);
            }
            else
            {
                if (_health > 100)
                {
                    slider.maxValue = _health;
                }
                slider.value = _health;
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
                OnGetMark?.Invoke();
            }

            if (col.gameObject.CompareTag("Life") && _health <= slider.maxValue - 2)
            {
                _health += 2;
                col.gameObject.SetActive(false);
            }
            if(col.gameObject.CompareTag("Life") && _health.Equals(slider.maxValue))
            {
                col.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _hasKey = PlayerPrefs.GetInt("hasKey") == 1;
            _hasMark = PlayerPrefs.GetInt("hasMark") == 1;
            _health = PlayerPrefs.GetInt("PlayerHealth", (int)_health);
            _spawnPoint = PlayerPrefs.GetString("spawnPoint");
            if (_spawnPoint == "right")
            {
                _playerMovement.DirX = -1;
                gameObject.transform.position = rightSpawnPoint.position;
            }
            else if (_spawnPoint == "left")
            {
                _playerMovement.DirX = 1;
                gameObject.transform.position = leftSpawnPoint.position;
            }
        }

        private void OnDisable()
        {
            PlayerPrefs.SetInt("hasKey", _hasKey ? 1 : 0);
            PlayerPrefs.SetInt("hasMark", _hasMark ? 1 : 0);
            PlayerPrefs.SetInt("PlayerHealth", (int)_health);
        }
        
    }
}
