using UnityEngine;
using UnityEngine.SceneManagement;

namespace vittorio {
    public class PalloSceneManager : MonoBehaviour {

        public static PalloSceneManager Instance { get; private set; }

        private void Awake() {
            
            if(Instance != null) {
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
