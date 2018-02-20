using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YourHand : MonoBehaviour
{
    public List<Image> EnemyHands;
    public List<Image> MyHands;
    public Image Enemy1;
    public Image Enemy2;
    public Image Enemy3;
    public Image Enemy4;
    public Image Enemy5;
    
    public Image Player1;
    public Image Player2;
    public Image Player3;
    public Image Player4;
    public Image Player5;
    
    public void Draw()
    {
        EnemyHands = new List<Image>()
        {
            Enemy1,
            Enemy2,
            Enemy3,
            Enemy4,
            Enemy5,
        };
        MyHands = new List<Image>
        {
            Player1,
            Player2,
            Player3,
            Player4,
            Player5,
        };
        EnemyHands.ForEach(LoadCard);
        MyHands.ForEach(LoadCard);
    }


    private void LoadCard(Image image)
    {
                CsvLoad.Import();
        var rand = Random.RandomRange(0, CsvLoad.CardNum-1);
        var path = CsvLoad.CsvDatas[rand][CsvLoad.path];
        path  = "Sprites/cards/" + path;
        path = path.Replace(".png","");
        Debug.Log(path);
            var sprite = Resources.Load<Sprite>(path);
            image.sprite = sprite; 
    }

}
