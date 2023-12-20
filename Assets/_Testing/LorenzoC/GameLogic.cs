using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using TMPro;
using UnityEngine;

namespace LorenzoCastelli { 

public class GameLogic : MonoBehaviour
{
        //public TextMeshProUGUI timerText;
         public float playTime;
         public GAMESTATES gameState = GAMESTATES.START;
        public  PalloController pallo;

        public  PlayerData[] playersList = new PlayerData[4];

        public  List<PlayerData> playerInGame = new List<PlayerData>();

    private static GameLogic instance;
    public enum GAMESTATES {
        START = 0,
        PLAYING = 1,
        TIMEUP = 2,
        ENDGAME = 3
    }

       

    private void Awake() {
        if (instance) {
            Destroy(this);
        } else {
            instance = this;
        }
    }

    private void Start() {
        EnterStart();
    }

        private  void EnterStart() {
            //timerText.text = "00";
            Debug.Log("Changing to " +  GAMESTATES.PLAYING + " from " + gameState);
            EnterPlaying();
    }

    private  void EnterPlaying() {
            //EFFETTO ENTRATA INIZIO PARTITA
            //InitPlayers()
            foreach(PlayerData player in playersList) {
                playerInGame.Add(player);
            }
            //TM.text = playTime.ToString();
        gameState = GAMESTATES.PLAYING;
    }

    private void EnterTimeUp() {
        gameState = GAMESTATES.TIMEUP;
        //EFFETTO ENTRATA OVERTIME
    }

    private static void EnterEndGame() {
        //EFFETTO ENTRATA FINE PARTITA
    }
    private static void UpdateOnGameLogic() {
        
    }

    private static void UpdateStart() {
        //ChangeGameLogic(GAMESTATES.PLAYING);
    }

    private void UpdatePlaying() {
        playTime -= Time.deltaTime;
            //TM.text = ((int)playTime).ToString();
            if (playTime <= 0) {
            EnterTimeUp();
        }
        //EFFETTI DA MANTENERE DURANTE PARTITA
    }

    private static void UpdateTimeUp() {
        //ROBE DA FAR FARE QUANDO IN OVERTIME
    }

    private static void UpdateEndGame() {
        //ROBE DA FAR FARE QUANDO IN FINE PARTITA
    }



    // Update is called once per frame
    void Update()
    {
            switch (gameState) {
                case GAMESTATES.START:
                    UpdateStart();
                    break;
                case GAMESTATES.PLAYING:
                    UpdatePlaying();
                    break;
                case GAMESTATES.TIMEUP:
                    UpdateTimeUp();
                    break;
                case GAMESTATES.ENDGAME:
                    UpdateEndGame();
                    break;
            }
        }

        private  PlayerData FindPlayer(PlayerData player) {
            return Array.Find(playersList, ele => ele.Equals(player));
        }

        public  bool CheckIfIsAlive(PlayerData player) {
            PlayerData pd = FindPlayer(player);
            return pd.isAlive();
        }
        
        public  void RemovePlayer(PlayerData player) {
            if (!CheckIfIsAlive(player)) {
                playerInGame.Remove(FindPlayer(player));
                IsOnlyOneAlive();
            }
        }
        
        private  void IsOnlyOneAlive() {
            if (playerInGame.Count == 1) {
                EnterEndGame(); 
            }
        }


    }





}