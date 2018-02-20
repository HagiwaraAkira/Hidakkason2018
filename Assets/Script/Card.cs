using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler,IDropHandler{
    public int Id;
    public int Level;
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;
    public Text TopText;
    public Text BottomText;
    public Text RightText;
    public Text LeftText;
    public Image Image;
    private Sprite _sprite;

    public bool Used;

    public RectTransform RectTransform;
    public Vector3 Position;

    public void Awake()
    {
               TopText.text = "";
        RightText.text = "";
        BottomText.text = "";
        LeftText.text = "";
    }
    public void Set(int top, int right, int bottom, int left, Sprite image)
    {
        Top = top;
        Right = right;
        Bottom = bottom;
        Left = left;
        _sprite = image;

        RectTransform = transform as RectTransform;
    }

    public void Show()
    {
               TopText.text = Top.ToString();
        RightText.text = Right.ToString();
        BottomText.text = Bottom.ToString();
        LeftText.text = Left.ToString(); 
        Image.sprite = _sprite;
    }

    public void OnBeginDrag(PointerEventData e)
    {
        Position = transform.localPosition;
    }

    public void OnDrag(PointerEventData e)
    {
        RectTransform.position = e.position;

    }

    public void OnEndDrag(PointerEventData e)
    {
        RectTransform.localPosition = Position;

        Observable.NextFrame().Where(_=>InGameController.SelectCell != "").Subscribe(_ =>
        {
            InGameController.Instance.SetCard(InGameController.SelectCell,this);
            InGameController.SelectCell = "";
        });
        
    }

    public void OnDrop(PointerEventData e)
    {
    }
}
