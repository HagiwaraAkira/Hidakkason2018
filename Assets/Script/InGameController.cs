using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameController : SingletonMonoBehaviour<InGameController>
{
	public static string SelectCell;
	public Field Field;
	public GameObject StartCut;
	public GameObject MyTurnCut;
	public GameObject EnemyTurnCut;

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
					Observable.Timer(TimeSpan.FromSeconds(YourHand.animationTime)).Subscribe(__ =>
					{
													var emptyCell = Field.CellList.FirstOrDefault(cell => cell.Card == null);
				var card = YourHand.EnemyHands.FirstOrDefault(hand => hand.Used == false);
				SetCard(emptyCell.name,card);		
					});

				});

			});

		RxState.Where(_ => _ == State.MyTurn).Subscribe(_ => { CutIn(MyTurnCut, () => { }); });
		
		CutIn(StartCut,()=>{
			YourHand.EnemyHands.ForEach(enemyhand=>enemyhand.Show());
								YourHand.MyHands.ForEach(myhand=>myhand.Show());
			CurrentState = State.MyTurn;
		});
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
