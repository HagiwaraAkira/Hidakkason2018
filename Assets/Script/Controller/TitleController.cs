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

	// Use this for initialization
	void Start ()
	{
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

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
