using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class InGameController : SingletonMonoBehaviour<InGameController>
{
	public static string SelectCell;
	public Field Field;
	
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
		YourHand.Draw();
		CurrentState = State.Draw;

		RxState.Where(_ => _ == State.EnemyTrun).Subscribe(_ =>
			{
				CutIn(EnemyTurnCut, () =>
				{
					Observable.Timer(TimeSpan.FromSeconds(YourHand.animationTime)).Subscribe(__ => { SetCard(); });

				});

			});

		RxState.Where(_ => _ == State.MyTurn).Subscribe(_ => { CutIn(MyTurnCut, () => { }); });
		
				RxState.Where(_ => _ == State.Finish).Subscribe(_ =>
				{
					var count = Field.CellList.Count(__ => __.IsMine);
					if (count > 4)
					{
						CutIn(WinCut, () =>
						{
							Observable.Timer(TimeSpan.FromSeconds(2f)).Subscribe(__ => { ResetGame();});							
						});
					}
					else
					{
						CutIn(LoseCut, () =>
						{
							Observable.Timer(TimeSpan.FromSeconds(2f)).Subscribe(__ => { ResetGame();});														
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
		NextState();
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
}

public static class LinqExtensions
{
	public static T RandomAt<T>(this IEnumerable<T> ie)
	{
		if (ie.Any() == false) return default(T);
		return ie.ElementAt(Random.Range(0, ie.Count()));
	}
}
