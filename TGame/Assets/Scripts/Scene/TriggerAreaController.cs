using Player;
using UnityEngine;

namespace Scene
{
    /**
     * Class invokes events if conditions are met
     */
    public class TriggerAreaController : MonoBehaviour
    {
        public delegate void Attack();
        public static event Attack OnAttack;
        public delegate void Idle();
        public static event Idle OnIdle;
        public delegate void BackToCenter();
        public static event BackToCenter OnBackToCenter;

        /**
         * depending on height on which is player, two events are invoked, OnAttack - nps starts attacking player, OnIdle - npc still follows a player but in idle state
         */
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

        /**
         * when player exits trigger area, npc stops attacking 
         */
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponent<PlayerInfo>())
            {
                OnBackToCenter?.Invoke();
            }
        }
    }
}
