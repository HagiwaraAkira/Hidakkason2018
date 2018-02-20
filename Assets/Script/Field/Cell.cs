using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.UIElements;
using Debug = UnityEngine.Debug;
using Image = UnityEngine.UI.Image;

public class Cell : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    public Sprite DefaultCell;
    public Sprite MyCell; 
    public Sprite EnemyCell;
    public Image Image;
    public bool IsMine;
    
    public Card Card;
        public void OnPointerEnter(PointerEventData eventData)
        {
                InGameController.SelectCell = gameObject.name;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (InGameController.SelectCell == gameObject.name)
            {
                InGameController.SelectCell = "";
            };
        }

    public void Awake()
    {
        Image = GetComponent<Image>();
    }

    public void ChangeImage(InGameController.State state)
    {
        switch(state)
        {case InGameController.State.EnemyTrun:

                Image.sprite = EnemyCell;
                                IsMine = false;
                break;
            case InGameController.State.MyTurn:
                                Image.sprite = MyCell;
                IsMine = true;
                break;
            default:
                                Image.sprite = DefaultCell;
                                IsMine = false;
                break;
        }
            
        
    }
    
}
