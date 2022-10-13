using System;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;

namespace NPC
{
    public class FireNpcController : MonoBehaviour
    {

        private int _encountersCount;
        public int EncountersCount
        {
            get => _encountersCount;
            set => _encountersCount = value;
        }
        private PlayerInfo _playerInfo;
        [SerializeField] private List<string> convoWhenHasKey = new List<string>();
        [SerializeField] private List<string> convoWhenHasKeyNot = new List<string>();
        [SerializeField] private TextMeshProUGUI panelText;

        private void Start()
        {
            _playerInfo = FindObjectOfType<PlayerInfo>();
        }

        public void ConvoController()
        {
            switch (_playerInfo.HasKey)
            {
                case true:
                    if (_encountersCount == 1)
                    {
                        panelText.text = convoWhenHasKey[0];
                    }else if (_encountersCount > 1)
                    {
                        panelText.text = convoWhenHasKey[1];
                    }
                    break;
                case false:
                    if (_encountersCount == 1)
                    {
                        panelText.text = convoWhenHasKeyNot[0];
                    }else if (_encountersCount > 1)
                    {
                        panelText.text = convoWhenHasKeyNot[1];
                    }
                    break;
            }
        }

        private void OnEnable()
        {
            _encountersCount = PlayerPrefs.GetInt("FireNpcEncounters");
        }

        private void OnDisable()
        {
            PlayerPrefs.SetInt("FireNpcEncounters", _encountersCount);
        }
        
    }
}
