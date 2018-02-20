using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour {
	
	public float RestTime = 30f;

	public bool isMine;

	public Slider RestSlider;
	public Text RestText;

	public IObservable<bool> RestObservable
	{
		get { return RestUnit; }
	}
	public Subject<bool> RestUnit = new Subject<bool>();
	
	// Update is called once per frame
	void Update () {
		if (!isMine &&InGameController.Instance.CurrentState == InGameController.State.EnemyTrun)
		{
			RestTime -= Time.deltaTime;
		}else if (isMine &&InGameController.Instance.CurrentState == InGameController.State.MyTurn)
		{
			RestTime -= Time.deltaTime;
		}

		RestSlider.value = RestTime / 30f;
		RestText.text = RestTime.ToString();
		if (RestTime <= 0)
		{
			RestUnit.OnNext(isMine);
		}
	}
	
}
