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

    public bool Used;

    public RectTransform RectTransform;
    public Vector3 Position;

    public void Set(int top, int right, int bottom, int left, Sprite image)
    {
        Top = top;
        Right = right;
        Bottom = bottom;
        Left = left;
        TopText.text = top.ToString();
        RightText.text = right.ToString();
        BottomText.text = bottom.ToString();
        LeftText.text = left.ToString();
        Image.sprite = image;
        RectTransform = transform as RectTransform;
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
