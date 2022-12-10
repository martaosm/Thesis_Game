using Player;
using UnityEngine;

namespace Scene
{
    /**
     * Class controls ramp behaviour in "Chapter1" scene
     */
    public class RampController : MonoBehaviour
    {
        [SerializeField] private GameObject point1;
        [SerializeField] private GameObject point2;
        private void OnCollisionStay2D(Collision2D collision)
        {
            //if player is on the ramp, they become child of the ramp object and ramp starts moving up to certain point
            if (collision.gameObject.GetComponent<PlayerInfo>())
            {
                collision.gameObject.transform.SetParent(transform);
                transform.position = Vector2.MoveTowards(transform.position, point1.transform.position, Time.deltaTime * 10);
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            //when player walk off the ramp, they stop being ramp child
            if (other.gameObject.GetComponent<PlayerInfo>())
            {
                other.gameObject.transform.SetParent(null);
            }
        }

        //when player is in trigger area, the ramp starts moving down to certain point
        private void OnTriggerStay2D(Collider2D col)
        {
            if (col.gameObject.GetComponent<PlayerInfo>())
            {
                transform.position = Vector2.MoveTowards(transform.position, point2.transform.position, Time.deltaTime * 10);
            }
        }
    }
}
