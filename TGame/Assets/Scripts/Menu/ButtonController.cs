using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Menu
{
    public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private List<GameObject> fireAnimation;

        //Detect if the Cursor starts to pass over the GameObject
        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            foreach (var fire in fireAnimation)
            {
                fire.SetActive(true);
            }
        }

        //Detect when Cursor leaves the GameObject
        public void OnPointerExit(PointerEventData pointerEventData)
        {
            foreach (var fire in fireAnimation)
            {
                fire.SetActive(false);
            }
        }
    }
}
