using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDirector : SingletonMonoBehaviour<SceneDirector> {
    public void Move(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
//        Debug.Log("hogehoge");
    }
}
