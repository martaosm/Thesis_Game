using System;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    /**
     * Class maintaining important information about player an their statistics, e.g. their hp, if player is in possession of a key etc.
     */
    public class PlayerInfo : MonoBehaviour
    {
        public float maxHealth;
        private float _health = 100;
        private bool _isAttacking;
        private bool _hasKey;
        private bool _hasMark;
        private bool _inputEnabled = true;
        private String _spawnPoint;
        private PlayerMovement _playerMovement;
        [SerializeField] private Slider slider;
        [SerializeField] private Transform rightSpawnPoint;
        [SerializeField] private Transform leftSpawnPoint;
        public delegate void GetMark();
        public static event GetMark OnGetMark;

        //public variable, player's HP
        public float Health
        {
            get => _health;
            set => _health = value;
        }
        
        /**
         * variable determines if player can move or not
         */
        public bool InputEnabled
        {
            get => _inputEnabled;
            set => _inputEnabled = value;

        }
        
        /**
         * method returns true if player is in possession of a key
         */
        public bool HasKey
        {
            get => _hasKey;
            set => _hasKey = value;
        }

        /**
         * method returns true if player is in possession of a mark
         */
        public bool HasMark
        {
            get => _hasMark;
            set => _hasMark = value;
        }

        /**
         * method returns true if player is attacking
         */
        public bool IsAttacking
        {
            get => _isAttacking;
            set => _isAttacking = value;
        }

        private void Update()
        {
            //controls player's health bar
            maxHealth = slider.maxValue;
            if (_health <= 0)
            {
                slider.gameObject.SetActive(false);
            }
            else
            {
                //if player gains more HP than maxValue then health bar max value changes
                if (_health > 100)
                {
                    slider.maxValue = _health;
                }
                slider.value = _health;
            }
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            //player obtain key object
            if (col.gameObject.CompareTag("Key"))
            {
                _hasKey = true;
                col.gameObject.SetActive(false);
            }
            
            //player obtain mark object
            if (col.gameObject.CompareTag("Mark"))
            {
                _hasMark = true;
                col.gameObject.SetActive(false);
                OnGetMark?.Invoke();
            }

            //if player's health bar is not full then player gains health after getting heart
            if (col.gameObject.CompareTag("Life") && _health <= slider.maxValue - 2)
            {
                _health += 2;
                col.gameObject.SetActive(false);
            }
            //but if health is full then heart disappears with no health gain 
            if(col.gameObject.CompareTag("Life") && _health.Equals(slider.maxValue))
            {
                col.gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            //gets player prefs
            _playerMovement = GetComponent<PlayerMovement>();
            _hasKey = PlayerPrefs.GetInt("hasKey") == 1;
            _hasMark = PlayerPrefs.GetInt("hasMark") == 1;
            _health = PlayerPrefs.GetInt("PlayerHealth", (int)_health);
            _spawnPoint = PlayerPrefs.GetString("spawnPoint");
            //spawning player in spawn point, depends from which direction player came from
            switch (_spawnPoint)
            {
                case "right":
                    _playerMovement.DirX = -1;
                    gameObject.transform.position = rightSpawnPoint.position;
                    break;
                case "left":
                    _playerMovement.DirX = 1;
                    gameObject.transform.position = leftSpawnPoint.position;
                    break;
            }
        }

        private void OnDisable()
        {
            //saves player prefs
            PlayerPrefs.SetInt("hasKey", _hasKey ? 1 : 0);
            PlayerPrefs.SetInt("hasMark", _hasMark ? 1 : 0);
            PlayerPrefs.SetInt("PlayerHealth", (int)_health);
        }
    }
}
