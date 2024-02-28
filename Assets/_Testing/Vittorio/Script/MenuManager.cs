using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;

namespace vittorio {
    public class MenuManager : MonoBehaviour {

        public static MenuManager Instance { get; private set; }
        public List<UIPlayer> uiPlayers = new List<UIPlayer>();
        public IMultiplayerInteractiveMenu activeMenu;

        public Action<int> OnPlayerCountChange;

        private void Awake() {

            if (Instance != null) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            uiPlayers = FindObjectsByType<UIPlayer>(FindObjectsSortMode.None).ToList();


        }


        public void AddPlayer(UIPlayer player) {
            if(uiPlayers.Contains(player)) return;

            uiPlayers.Add(player);
        }

        public void RemovePlayer(UIPlayer player) {
            uiPlayers.Remove(player);
        }

        public void LoadScene(int sceneID) {
            SceneManager.LoadScene(sceneID);

        }


    }
}
