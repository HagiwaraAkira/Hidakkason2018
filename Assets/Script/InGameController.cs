using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InGameController : SingletonMonoBehaviour<InGameController>
{
	public static string SelectCell = "";
	public Field Field;
	public Status Status;
	
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

	public TimeCounter MyTurnCounter;
	public TimeCounter EnemyTurnCounter;

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
		MyTurnCounter.RestObservable.Subscribe(_ =>
		{
			SetCard();
		});
		
		EnemyTurnCounter.RestObservable.Subscribe(_ =>
		{
			SetCard();
		});
		
		SetModeText();
		SetResult();
		YourHand.Draw();
		CurrentState = State.Draw;

		RxState.Where(_ => _ == State.EnemyTrun).Subscribe(_ =>
			{
				EnemyTurnIcon.SetActive(true);
				PlayerTurnIcon.SetActive(false);
				CutIn(EnemyTurnCut, () =>
				{
					Observable.Timer(TimeSpan.FromSeconds(YourHand.animationTime)).Subscribe(__ =>
					{
						SetCard(true);
					});

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

	private void SetCard(bool anim = false)
	{
		Card card = null;
		Cell emptyCell = null;
		switch (Setting.Instance.EnemyMode)
		{

			case 0:
			  	emptyCell = Field.CellList.Where(cell => cell.Card == null).RandomAt();
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
		if (anim)
		{
		card.AnimationX();
			
		}
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

		MyTurnCounter.RestTime = 30f;
				EnemyTurnCounter.RestTime = 30f;
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
					if(targetCell.IsMine && CurrentState == State.EnemyTrun || !targetCell.IsMine && CurrentState == State.MyTurn)
					targetCell.Card.AnimationX();
					targetCell.ChangeImage(CurrentState);
				}
			}
		}
		// 上
			if (index > 2)
			{
				var targetCell2 = Field.CellList[index - 3];
				if (targetCell2.Card != null)
				{
					if (targetCell2.Card.Bottom < cell.Card.Top)
					{
						if(targetCell2.IsMine && CurrentState == State.EnemyTrun || !targetCell2.IsMine && CurrentState == State.MyTurn)
						targetCell2.Card.AnimationY();
						targetCell2.ChangeImage(CurrentState);
					}
				}
			}

			// 下
		if (index < 6)
		{
			var targetCell3 = Field.CellList[index + 3];
			if (targetCell3.Card != null)
			{
				if (targetCell3.Card.Top < cell.Card.Bottom)
				{
					if(targetCell3.IsMine && CurrentState == State.EnemyTrun || !targetCell3.IsMine && CurrentState == State.MyTurn)
					targetCell3.Card.AnimationY();
					targetCell3.ChangeImage(CurrentState);
				}
			}
		}

		//右
			if (index % 3 != 2)
			{
				var targetCell4 = Field.CellList[index + 1];
				if (targetCell4.Card != null)
				{
					if (targetCell4.Card.Left < cell.Card.Right)
					{
						if(targetCell4.IsMine && CurrentState == State.EnemyTrun || !targetCell4.IsMine && CurrentState == State.MyTurn)
							targetCell4.Card.AnimationX();
						targetCell4.ChangeImage(CurrentState);
					}
				}

			}

		var count = Field.CellList.Count(_=>_.IsMine);
		count += YourHand.MyHands.Count(_=>_.Used==false);
		Status.SetScore(count);
				NextState();
		}
	
	
	#region Mode visible

	public Text ModeText;

	public void SetModeText()
	{
		var mode = "";
		if (Setting.Instance.SameMode)
		{
			mode += "SAME";
		}


		if (Setting.Instance.PlusMode)
		{
					if (mode != "")
		{
			mode += " + ";
		}
			mode += "PLUS";
		}

		if (Setting.Instance.ReverseMode)
		{
						if (mode != "")
		{
			mode += " + ";
		}
			mode += "REVERSE";
		}

		ModeText.text = mode;
	}

	#endregion
}

public static class LinqExtensions
{
	public static T RandomAt<T>(this IEnumerable<T> ie)
	{
		if (ie.Any() == false) return default(T);
		return ie.ElementAt(Random.Range(0, ie.Count()));
	}
}
