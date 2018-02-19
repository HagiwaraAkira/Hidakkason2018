using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour{

	private static T _instance;
	public static T Instance
	{
		get{
			if (_instance == null) {
				var t = typeof(T);

				_instance = (T)FindObjectOfType (t);
				if (_instance == null) {
					var go = new GameObject(typeof(T).ToString());
					_instance = go.AddComponent<T>();
					DontDestroyOnLoad(go);
				}
			}

			return _instance;
		}
	}

	protected virtual void Awake(){
		// 他のゲームオブジェクトにアタッチされているか調べる
		// アタッチされている場合は破棄する。
		CheckInstance();
	}

	private void CheckInstance(){
		if (_instance == null)
		{
			_instance = this as T;
			return;
		} else if (Instance == this)
		{
			return;
		}
		Destroy (this);
		return;
	}
}