using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Examples;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class YourHand : MonoBehaviour
{
    public float animationTime = 0.1f;
    public List<Card> EnemyHands;
    public List<Card> MyHands;
    public Card Enemy1;
    public Card Enemy2;
    public Card Enemy3;
    public Card Enemy4;
    public Card Enemy5;
    
    public Card Player1;
    public Card Player2;
    public Card Player3;
    public Card Player4;
    public Card Player5;
    
    public void Draw()
    {
        EnemyHands = new List<Card>()
        {
            Enemy1,
            Enemy2,
            Enemy3,
            Enemy4,
            Enemy5,
        };
        MyHands = new List<Card>
        {
            Player1,
            Player2,
            Player3,
            Player4,
            Player5,
        };
        
        int count = 0;
        EnemyHands.ForEach(_ =>
        {
            _.gameObject.SetActive(false);
            Observable.Timer(TimeSpan.FromSeconds(count *animationTime)).Subscribe(__ =>
            {
                LoadCard(_);
                DrawEnemyAnimation(_.gameObject);
            });
                
            count++; 

        });
        count = 0;
        MyHands.ForEach(_ =>
        {
            _.gameObject.SetActive(false);
            Observable.Timer(TimeSpan.FromSeconds(count *animationTime)).Subscribe(__ =>
            {
                LoadCard(_);
                DrawAnimation(_.gameObject);
            });
            count++;
        });
    }

    public void DrawAnimation(GameObject o)
    {
        iTween.MoveFrom(o, 
            iTween.Hash(
                "x",0,
                "y",0,
                "easeType",iTween.EaseType.linear,
                "time",animationTime
            )
        );
        o.SetActive(true);
        iTween.RotateAdd(o, iTween.Hash("z", 360f, "time", animationTime,            "easeType",iTween.EaseType.linear));
    }
    public void DrawEnemyAnimation(GameObject o)
    {
        iTween.MoveFrom(o, 
            iTween.Hash(
                "x",1136,
                "y",0,
                "easeType",iTween.EaseType.linear,
                "time",animationTime
            )
        );
        o.SetActive(true);
        iTween.RotateAdd(o, iTween.Hash("z", 360f, "time", animationTime,            "easeType",iTween.EaseType.linear));
    }


    private void LoadCard(Card card)
    {
        CsvLoad.Import();
        var rand = Random.RandomRange(1, CsvLoad.CardNum-1);
        var path = CsvLoad.CsvDatas[rand][CsvLoad.path];
        path  = "Sprites/cards/" + path;
        path = path.Replace(".png","");
//        Debug.Log(path);
        
            var sprite = Resources.Load<Sprite>(path);
        var top =  int.Parse( CsvLoad.CsvDatas[rand][CsvLoad.top]);
                var right =  int.Parse( CsvLoad.CsvDatas[rand][CsvLoad.right]);
                var bottom =  int.Parse( CsvLoad.CsvDatas[rand][CsvLoad.bottom]);
                var left =  int.Parse( CsvLoad.CsvDatas[rand][CsvLoad.left]);
            card.Set(top,right,bottom,left,sprite); 
        
    }

}
