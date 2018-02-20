using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
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
    }
}
