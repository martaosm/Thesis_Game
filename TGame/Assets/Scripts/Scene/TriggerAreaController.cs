using System;
using NPC;
using Player;
using UnityEngine;

namespace Scene
{
    public class TriggerAreaController : MonoBehaviour
    {
        private KleptomaniacController _kleptoNpcController;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject kleptoNpc;
        public delegate void Attack();
        public static event Attack OnAttack;
        public delegate void Idle();
        public static event Idle OnIdle;

        public delegate void BackToCenter();

        public static event BackToCenter OnBackToCenter;

        private void Start()
        {
            _kleptoNpcController = kleptoNpc.GetComponent<KleptomaniacController>();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.GetComponent<PlayerInfo>() && other.transform.position.y>=9.338f && other.transform.position.y<=9.34f)
            {
                OnAttack?.Invoke();
                
            }
            if (other.GetComponent<PlayerInfo>() && other.transform.position.y>=9.34f)
            {
                OnIdle?.Invoke();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponent<PlayerInfo>())
            {
                OnBackToCenter?.Invoke();
            }
        }
    }
}
