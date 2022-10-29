using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene
{
    public class SceneController : MonoBehaviour
    {
        private bool _nextChamber;
        private float _dirX;
        private CameraController _cameraController;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject checkpoint;

    
        private void Start()
        {
            _cameraController = FindObjectOfType<CameraController>();
        }

        private void Update()
        {
            if (_nextChamber)
            {
                player.GetComponent<Rigidbody2D>().velocity =
                    new Vector2(_dirX * 10, player.GetComponent<Rigidbody2D>().velocity.y);
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.GetComponent<PlayerInfo>())
            {
                player.GetComponent<PlayerInfo>().InputEnabled = false;
                if (Input.GetAxisRaw("Horizontal")>0)
                {
                    _dirX = 1;
                }else if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    _dirX = -1;
                }
                //_dirX = Input.GetAxisRaw("Horizontal");
                _cameraController.enabled = false;
                _nextChamber = true;
                StartCoroutine(MoveToNextChamber());
            }
        }

        IEnumerator MoveToNextChamber()
        {
            yield return new WaitForSeconds(3f);
            if (_dirX > 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }else if (_dirX < 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            }
        }
    }
}
