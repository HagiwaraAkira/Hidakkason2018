using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    public Card Card;
        public void OnPointerEnter(PointerEventData eventData)
        {
                InGameController.SelectCell = gameObject.name;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (InGameController.SelectCell == gameObject.name)
            {
                InGameController.SelectCell = gameObject.name;
            };
        }
}
