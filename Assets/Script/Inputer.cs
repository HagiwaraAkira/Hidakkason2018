using UnityEngine;

public class Inputer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("aaa");
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Move()
	{
		SceneDirector.Instance.Move("Ingame");
		SoundManager.Instance.PlaySe("button");
	}
}
