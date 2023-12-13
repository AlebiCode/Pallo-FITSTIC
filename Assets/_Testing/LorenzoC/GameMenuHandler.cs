using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace LorenzoCastelli {


public class GameMenuHandler : MonoBehaviour {

        public GameObject UIItemMenu;

        public PlayableDirector cameraAnim;
        public Animation oneTimeAnimHandler;
        public AnimationClip customizeAnim;
        
        public void Customize() {
            UIItemMenu.active = false;
            cameraAnim.Stop();
            oneTimeAnimHandler.clip = customizeAnim;
            oneTimeAnimHandler.Play();
    }

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
}