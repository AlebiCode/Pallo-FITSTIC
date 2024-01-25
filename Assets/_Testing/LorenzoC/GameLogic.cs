using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using TMPro;
using UnityEngine;

namespace LorenzoCastelli {

    public class GameLogic : MonoBehaviour {

        public TextMeshProUGUI timerText;
        public float playTime;
        public GAMESTATES gameState = GAMESTATES.START;
        public PalloController pallo;

        public PlayerData[] playersList = new PlayerData[4];

        public List<PlayerData> playerInGame = new List<PlayerData>();

        public Transform[] arenaAreas;

        private GameObject[] areasOccupied;

        public static GameLogic instance;
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
            GameObject[] temp = new GameObject[arenaAreas.Length];
           
           for (int i=0; i< arenaAreas.Length; i++) {
                temp[i] = null;
            }
            areasOccupied = temp;
        }

        public void ForceLookTarget(PlayerData initializer) {
            foreach (PlayerData player in playerInGame) {
                if ((player != initializer) && (player.gameObject.GetComponent<CPUController>())) {
                    player.GetComponent<CPUController>().currentLookTarget = initializer.gameObject;
                    ForceDistanceBots(initializer);
                }
            }
        }

        public void ForceLookTarget() {
            foreach (PlayerData player in playerInGame) {
                if (player.gameObject.GetComponent<CPUController>()) {
                    player.GetComponent<CPUController>().currentLookTarget = null;
                }
            }
        }

        private void Start() {
            EnterStart();
        }

        private void EnterStart() {
            timerText.text = "00";
            Debug.Log("Changing to " + GAMESTATES.PLAYING + " from " + gameState);
            EnterPlaying();
        }

        private void EnterPlaying() {
            //EFFETTO ENTRATA INIZIO PARTITA
            //InitPlayers()
            foreach (PlayerData player in playersList) {
                playerInGame.Add(player);
            }
            timerText.text = playTime.ToString();
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
        void Update() {
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

        private PlayerData FindPlayer(PlayerData player) {
            return Array.Find(playersList, ele => ele.Equals(player));
        }

        public bool CheckIfIsAlive(PlayerData player) {
            PlayerData pd = FindPlayer(player);
            return pd.isAlive();
        }

        public void RemovePlayer(PlayerData player) {
            if (!CheckIfIsAlive(player)) {
                playerInGame.Remove(FindPlayer(player));
                IsOnlyOneAlive();
            }
        }

        private void IsOnlyOneAlive() {
            if (playerInGame.Count == 1) {
                EnterEndGame();
            }
        }

        /*public void ReorderList() {
            Debug.Log("Entering");
            List<PlayerData> tmp = new List<PlayerData>();
            tmp.Add(playerInGame[0]);
            for (int i = 1; i < 4; i++) {
                for (int j = 0; j < tmp.Count; j++) {
                    if (playerInGame[i].importance > tmp[j].importance) {
                        PlayerData change = playerInGame[j];
                        tmp[j] = playerInGame[i];
                        tmp.Add(change);
                        break;
                    }

                }
                tmp.Add(playerInGame[i]);
            }
            playerInGame = null;
            playerInGame = tmp;
        }*/


        public bool IsPlayerClose(PlayerData caller, PlayerData otherPlayer) {
            if (otherPlayer.GetComponent<PlayerData>()) {
                PlayerData close = playerInGame.Find(player => player == otherPlayer);
                if (close) {
                    if (Vector3.Distance(close.gameObject.transform.position, caller.gameObject.transform.position) <= 0.5f) {
                        return true;
                    }
                       
                    }
                }
            return false;
            }

        public PlayerData GetClosestMostImportantPlayer(PlayerData player) {
            float distance = player.lookDistance;
            int maxImportance = 0;
            PlayerData target = null;
            foreach (PlayerData players in playerInGame) {
                if ((Vector3.Distance(players.transform.position, player.transform.position) < distance) && (players.importance > maxImportance) && players != player) {
                    target = players;
                    maxImportance = players.importance;
                    distance = Vector3.Distance(players.transform.position, player.transform.position);
                }
            }
            if (maxImportance > 0) {
                return target;
            } else {
                return null;
            }
        }

        public PlayerData FindInterestingPlayer(PlayerData player) {
            PlayerData target = null;
            float maxHeavyPoints = 0;
            foreach (PlayerData pd in playerInGame) {
                if ((pd != player) && (CalculateHeavyPoint(pd) / Vector3.Distance(player.transform.position, pd.transform.position) >= maxHeavyPoints)) {
                    target = pd;
                    maxHeavyPoints = CalculateHeavyPoint(pd);
                }
            }
            return target;
        }

        private float CalculateHeavyPoint(PlayerData pd) {
            return pd.importance / pd.currentHp;
        }


        public void ForceDistanceBots(PlayerData from) {
            foreach (PlayerData player in playerInGame) {
                if (player.GetComponent<CPUController>() && player != from) {
                    player.GetComponent<CPUController>().ChangeState(PLAYERSTATES.BACKINGOFF);
                    //player.GetComponent<CPUController>().BackingOff();
                }
            }
        }

        public GameObject ReturnClosestEnemyFromBall() {
            PlayerData target = null;
            float distance=0.5f;
            foreach(PlayerData player in playerInGame) {
                if (Vector3.Distance(player.transform.position, pallo.transform.position) < distance) {
                    target = player;
                    distance = Vector3.Distance(player.transform.position, pallo.transform.position);
                }
            }
            if (distance >= 0.5) {
                return null;
            } else {
                return target.gameObject;
            }
        }

        public void ClearPlayerInArea(GameObject player) {
            for (int i = 0; i < arenaAreas.Length; i++) { 
                if (player == areasOccupied[i]) {
                    areasOccupied[i] = null;
                    return;
                }
            }
            Debug.LogWarning("Player " + player + " was not in the area");
            }

        public Transform FindFarestPoint(GameObject caller , GameObject from) {
            float distance = 0;
            Transform finalPos = from.transform;
            int areaToOccupy=-1;
            for (int i = 0; i < arenaAreas.Length; i++) { 
                if ((Vector3.Distance(finalPos.position, arenaAreas[i].position) > distance) && (!areasOccupied[i])) {
                    distance = Vector3.Distance(finalPos.position, arenaAreas[i].position);
                    finalPos = arenaAreas[i];
                    areaToOccupy = i;
                }
            }
            if (areaToOccupy > -1) {
            areasOccupied[areaToOccupy] = caller;
            }
            return finalPos;
        }

    }
}