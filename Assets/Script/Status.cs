using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
	public List<Image> Score;
	public Sprite MySprite;
	public Sprite EnemySprite;


	// Use this for initialization
	void Start () {
		
		SetScore(5);
	}

	public void SetScore(int myCount)
	{
		for (var i = 0; i < 10; i++)
		{

			if (i < myCount)
			{
   			   Score[i].sprite = MySprite;
			}
			else
			{
   			   Score[i].sprite = EnemySprite;				
			}
				
		}
		
		
		
	}
	

}
