using UnityEngine;

namespace Camera
{ 
    /**
     * Class responsible for managing a camera and keeping it centered on player
     * */
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform player;
    
        private void Update()
        {
            var transform1 = transform;
            var position = player.position;
            transform1.position = new Vector3(position.x, position.y+4, transform1.position.z);
        }
    }
}
