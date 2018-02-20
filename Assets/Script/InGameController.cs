﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InGameController : SingletonMonoBehaviour<InGameController>
{
	public static string SelectCell;
	public Field Field;
	
	[Header("Turn")]
	public GameObject PlayerTurnIcon;
	public GameObject EnemyTurnIcon;	
	[Header("cut in")]
	public GameObject StartCut;
	public GameObject MyTurnCut;
	public GameObject EnemyTurnCut;
	
	public GameObject WinCut;
	public GameObject LoseCut;
	public GameObject DrawCut;

	public enum State
	{
		Draw,
		MyTurn,
		EnemyTrun,
		Finish
	}

	public State CurrentState
	{
		get { return RxState.Value; }
		set { RxState.Value = value; }
	}

	public ReactiveProperty<State> RxState = new ReactiveProperty<State>();

	public YourHand YourHand;
	// Use this for initialization
	void Start ()
	{
		SetResult();
		YourHand.Draw();
		CurrentState = State.Draw;

		RxState.Where(_ => _ == State.EnemyTrun).Subscribe(_ =>
			{
				EnemyTurnIcon.SetActive(true);
				PlayerTurnIcon.SetActive(false);
				CutIn(EnemyTurnCut, () =>
				{
					Observable.Timer(TimeSpan.FromSeconds(YourHand.animationTime)).Subscribe(__ => { SetCard(); });

				});

			});

		RxState.Where(_ => _ == State.MyTurn).Subscribe(_ =>
		{
							EnemyTurnIcon.SetActive(false);
				PlayerTurnIcon.SetActive(true);
			CutIn(MyTurnCut, () => { });
		});
		
				RxState.Where(_ => _ == State.Finish).Subscribe(_ =>
				{
					var count = Field.CellList.Count(__ => __.IsMine);
					if (count > 5)
					{
						CutIn(WinCut, () =>
						{
							Observable.Timer(TimeSpan.FromSeconds(2f)).Subscribe(__ => { ResetGame();});
							Setting.Instance.Win++;
						});
					}
					else if (count == 5)
					{ 
						CutIn(DrawCut, () =>
						{
							Observable.Timer(TimeSpan.FromSeconds(2f)).Subscribe(__ => { ResetGame();});
							Setting.Instance.Draw++;
						});
					}
					else
					{
						CutIn(LoseCut, () =>
						{
							Observable.Timer(TimeSpan.FromSeconds(2f)).Subscribe(__ => { ResetGame();});
							Setting.Instance.Lose++;
						});	
					}
				});
		
		CutIn(StartCut,()=>{
			EnemyHandShow();
			YourHand.MyHands.ForEach(myhand=>myhand.Show());
			CurrentState = State.MyTurn;
		});
	}

	private void EnemyHandShow()
	{
		switch (Setting.Instance.VisibleMode)
		{
			case 0:
				YourHand.EnemyHands.ForEach(enemyhand=>enemyhand.Show());
				break;
			case 1:
				break;
			case 2:
				foreach (var hand in YourHand.EnemyHands.Take(3))
				{
					hand.Show();
				}
				break;
		}
		
	}

	private void SetCard()
	{
		Card card = null;
		Cell emptyCell = null;
		switch (Setting.Instance.EnemyMode)
		{

			case 0:
			  	emptyCell = Field.CellList.FirstOrDefault(cell => cell.Card == null);
				 card = YourHand.EnemyHands.Where(hand => hand.Used == false).RandomAt();
				break;
			case 1:
				emptyCell = Field.CellList.FirstOrDefault(cell => cell.Card == null);
				 card = YourHand.EnemyHands.FirstOrDefault(hand => hand.Used == false);
				break;
			case 3:
				return;
				break;
			default:
				emptyCell = Field.CellList.FirstOrDefault(cell => cell.Card == null);
				card = YourHand.EnemyHands.FirstOrDefault(hand => hand.Used == false);
				break;
		}
		

		SetCard(emptyCell.name,card);				
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
		Judge(cellName);
		card.Show();
	}

	public void NextState()
	{
	var emptyCell = Field.CellList.FirstOrDefault(cell => cell.Card == null);
		if (emptyCell == null)
		{
						CurrentState = State.Finish;
		}else	if (CurrentState == State.EnemyTrun)
		{
			CurrentState = State.MyTurn;
		}
		else
		{
			CurrentState = State.EnemyTrun;	
		}
	}

	public void CutIn(GameObject cutin,Action callback)
	{
		Observable.Timer(TimeSpan.FromSeconds(YourHand.animationTime * 5)).Subscribe(_ =>
		{
			cutin.SetActive(true);
			Observable.Timer(TimeSpan.FromSeconds(YourHand.animationTime * 5)).Subscribe(__ =>
			{
				cutin.SetActive(false);
				callback();
			});
		});
	}

	public Text WinText;
	public Text LoseText;
	public Text DrawText;
	public Text TotalText;

	private void SetResult()
	{
		WinText.text = Setting.Instance.Win.ToString();
		LoseText .text = Setting.Instance.Lose.ToString();
				DrawText .text = Setting.Instance.Draw.ToString();
				TotalText.text = (Setting.Instance.Win+Setting.Instance.Lose).ToString();
	}

	public void Judge(string cellName)
	{
		var cell = Field.GetCell(cellName);
		var index = Field.CellList.IndexOf(cell);
		// 左側比較
		if (index % 3 != 0)
		{
			var targetCell = Field.CellList[index - 1];
			if (targetCell.Card != null)
			{
				
			if (targetCell.Card.Right < cell.Card.Left)
			{
				targetCell.ChangeImage(CurrentState);
			}
				
			}
		}
		Debug.Log(cellName);
		NextState();
		
	}
}

public static class LinqExtensions
{
	public static T RandomAt<T>(this IEnumerable<T> ie)
	{
		if (ie.Any() == false) return default(T);
		return ie.ElementAt(Random.Range(0, ie.Count()));
	}
}
