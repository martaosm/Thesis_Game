using UnityEngine;

namespace Scene
{
    /**
     * Class to not destroy object that controls background music
     */
    public class DoNotDestroy : MonoBehaviour
    {
        /**
         * music plays smoothly during whole game, when track ends it starts to play again
         */
        private void Awake()
        {
            var musicObj = GameObject.FindGameObjectsWithTag("GameMusic");
            if (musicObj.Length > 1)
            {
                Destroy(this.gameObject);
            }
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
