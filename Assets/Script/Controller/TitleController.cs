using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleController : MonoBehaviour
{
	public Button PlayButton;
	public Button SettingButton;
	public Button SettingCloseButton;
	public GameObject SettingPanle;
	public static bool isSetting;


	public Toggle SameToggle;
	public Toggle PlusToggle;
	public Toggle ReverseToggle;
	
	public Toggle RandomToggle;
	public Toggle OrderToggle;
	public Toggle AiToggle;
	public Toggle ManualToggle;
	
	public Toggle OpenToggle;
	public Toggle CloseToggle;
	public Toggle ThreeToggle;	
	// Use this for initialization
	void Start ()
	{
		Setting.Instance.EnemyMode = 0;
		Setting.Instance.VisibleMode = 0;
		Setting.Instance.PlusMode = false;
		Setting.Instance.SameMode = false;
		Setting.Instance.ReverseMode = false;
		PlayButton.OnClickAsObservable().Subscribe(_ => { SceneManager.LoadScene("InGame"); });
		SettingButton.OnClickAsObservable().Subscribe(_ =>
		{
			SettingPanle.gameObject.SetActive(true);
		});
		
		SettingCloseButton.OnClickAsObservable().Subscribe(_ =>
		{
			SettingPanle.gameObject.SetActive(false);
		});

		if (isSetting)
		{
			SettingPanle.gameObject.SetActive(true);	
		}

		SameToggle.OnValueChangedAsObservable().Subscribe(_ => { Setting.Instance.SameMode = _;});
		PlusToggle.OnValueChangedAsObservable().Subscribe(_ => { Setting.Instance.PlusMode = _;});
		ReverseToggle.OnValueChangedAsObservable().Subscribe(_ => { Setting.Instance.ReverseMode = _;});
		
		OrderToggle.OnValueChangedAsObservable().Subscribe(_ => { Setting.Instance.EnemyMode = 1;});
		AiToggle.OnValueChangedAsObservable().Subscribe(_ => { Setting.Instance.EnemyMode = 2;});
		ManualToggle.OnValueChangedAsObservable().Subscribe(_ => { Setting.Instance.EnemyMode = 3;});
		RandomToggle.OnValueChangedAsObservable().Subscribe(_ => { Setting.Instance.EnemyMode = 0;});
		
		CloseToggle.OnValueChangedAsObservable().Subscribe(_ => { Setting.Instance.VisibleMode = 1;});
		ThreeToggle.OnValueChangedAsObservable().Subscribe(_ => { Setting.Instance.VisibleMode = 2;});
		OpenToggle.OnValueChangedAsObservable().Subscribe(_ => { Setting.Instance.VisibleMode = 0;});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
