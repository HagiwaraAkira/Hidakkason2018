using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameController : SingletonMonoBehaviour<InGameController>
{
	public static string SelectCell;
	public Field Field;

	public YourHand YourHand;
	// Use this for initialization
	void Start ()
	{
		YourHand.Draw();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Draw()
	{
		
	}

	public void ResetGame()
	{
		TitleController.isSetting = true;
		SceneManager.LoadScene("Title");
	}

	public void SetCard(string cellName, Card card)
	{
		Field.SetCard(cellName,card);
	}
}
