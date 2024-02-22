using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace vittorio {
    public class MenuManager : MonoBehaviour {

        public static MenuManager Instance { get; private set; }
        public List<UIPlayer> menuPlayers = new List<UIPlayer>();

        private void Awake() {

            if (Instance != null) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);



        }



        public void LoadScene(int sceneID) {
            SceneManager.LoadScene(sceneID);

        }


    }
}
