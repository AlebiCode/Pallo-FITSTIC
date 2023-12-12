using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuHandler : MonoBehaviour {


 public void Play() {
        ChangeScene("Test_Scene_Lory");
    }


    public void Quit() {
        Application.Quit();
    }
    private void ChangeScene(string SceneName) {
        Debug.Log("Loading " + SceneName);
        SceneManager.LoadScene(SceneName);
    }

}
