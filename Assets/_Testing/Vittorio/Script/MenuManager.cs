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
        public static List<UIPlayer> players = new List<UIPlayer>();
        public IMultiplayerInteractiveMenu activeMenu;

        public Action<int> OnPlayerCountChange;

        private void Awake() {

            if (Instance != null) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            players = FindObjectsByType<UIPlayer>(FindObjectsSortMode.None).ToList();


        }


        public void AddPlayer(UIPlayer player) {
            if(players.Contains(player)) return;

            players.Add(player);
        }

        public void RemovePlayer(UIPlayer player) {
            players.Remove(player);
        }

        public void LoadScene(int sceneID) {
            SceneManager.LoadScene(sceneID);

        }


    }
}
