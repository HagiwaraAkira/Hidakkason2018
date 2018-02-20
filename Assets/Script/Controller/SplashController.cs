using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashController : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		Observable.Timer(TimeSpan.FromSeconds(3)).Subscribe((_) =>
		{
			SceneManager.LoadScene("Title");
		});

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
