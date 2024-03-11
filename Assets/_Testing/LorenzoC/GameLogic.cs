using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using TMPro;
using UnityEngine;
using vittorio;

namespace LorenzoCastelli {

    public class GameLogic : MonoBehaviour {

        public PalloController pallo;


        public IPlayer[] playersList = new IPlayer[4];

        [SerializeField] private Transform playersContainer;
        public List<IPlayer> playerInGame = new List<IPlayer>();

        public Transform[] arenaAreasPositions;

        public GameObject[] areasOccupied;

        public static GameLogic instance;

        private void Awake() {
            if (instance) {
                Destroy(this);
            } else {
                instance = this;
            }

            playersList = playersContainer.GetComponentsInChildren<IPlayer>();


            GameObject[] temp = new GameObject[arenaAreasPositions.Length];
            for (int i=0; i< arenaAreasPositions.Length; i++) {
                temp[i] = null;
            }
            areasOccupied = temp;
        }
        private void Start() {
            EnterStart();
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


        private void EnterStart() {
            //Debug.Log("Changing to " + GAMESTATES.PLAYING + " from " + gameState);
            EnterPlaying();
        }

        private void EnterPlaying() {
            //EFFETTO ENTRATA INIZIO PARTITA
            //InitPlayers()
            foreach (IPlayer player in playersList) {
                playerInGame.Add(player);
            }
        }

        #region Player Handling

        private IPlayer FindPlayer(IPlayer player) {
            return Array.Find(playersList, ele => ele.Equals(player));
        }

        public bool CheckIfIsAlive(IPlayer player) {
            IPlayer pd = FindPlayer(player);
            return pd.IsAlive;
        }

        public void RemovePlayer(IPlayer player) {
            if (!CheckIfIsAlive(player)) {
                playerInGame.Remove(FindPlayer(player));
                IsOnlyOneAlive();
            }
        }

        private void IsOnlyOneAlive() {
            if (playerInGame.Count == 1) {
                EndGame();
            }
        }

        private void EndGame()
        {
            //TODO
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
                IPlayer close = playerInGame.Find(player => player == otherPlayer);
                if (close != null) {
                    if (Vector3.Distance(((MonoBehaviour)close).transform.position, caller.gameObject.transform.position) <= 0.5f) {
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

        public IPlayer FindInterestingPlayer(IPlayer player) {
            IPlayer target = null;
            float maxHeavyPoints = -100;
            Debug.Log("Wtf " + playerInGame.Count);
            foreach (IPlayer pd in playerInGame) {
                float curHeavyPoints = CalculateHeavyPoint(pd) + Vector3.Distance(((MonoBehaviour)player).transform.position, ((MonoBehaviour)pd).transform.position);
                if ((pd != player) && (curHeavyPoints >= maxHeavyPoints)) {
                    target = pd;
                    maxHeavyPoints = curHeavyPoints;
                }
            }
            return target;
        }


        private float CalculateHeavyPoint(IPlayer pd) {
            Debug.Log("bruh " + ((MonoBehaviour)pd).name);
            return pd.Importance - pd.CurrentHp;
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
            float distance=100f;
            foreach(PlayerData player in playerInGame) {
                if (Vector3.Distance(player.transform.position, pallo.transform.position) < distance) {
                    target = player;
                    distance = Vector3.Distance(player.transform.position, pallo.transform.position);
                }
            }
            if (distance >= 100f) {
                return null;
            } else {
                return target.gameObject;
            }
        }

        public bool IsPlayerInArea(GameObject player) {
            for (int i = 0; i < arenaAreasPositions.Length; i++) {
                if (player == areasOccupied[i]) {
                    return true;
                    //Debug.LogWarning("Cleared " + player + " from area" + areasOccupied[i]);
                    
                }
            }
            return false;

        }

        public void ClearPlayerInArea(GameObject player) {
            for (int i = 0; i < arenaAreasPositions.Length; i++) { 
                if (player == areasOccupied[i]) {
                    areasOccupied[i] = null;
                    //Debug.LogWarning("Cleared " + player + " from area" + areasOccupied[i]);
                    return;
                } 
            }

            }

        public Transform FindFarestPoint(GameObject caller , GameObject from) {
            float distance = 0;
            Transform finalPos = from.transform;
            int areaToOccupy=-1;
            for (int i = 0; i < arenaAreasPositions.Length; i++) { 
                if ((Vector3.Distance(finalPos.position, arenaAreasPositions[i].position) > distance) && (!areasOccupied[i])) {
                    distance = Vector3.Distance(finalPos.position, arenaAreasPositions[i].position);
                    finalPos = arenaAreasPositions[i];
                    areaToOccupy = i;
                }
            }
            if (areaToOccupy > -1) {
            areasOccupied[areaToOccupy] = caller;
            }
            return finalPos;
        }

        public Vector3 FindFarestPointV2(GameObject ballPosition, IPlayer excludePlayer) {
            float distance = 0;
            Transform finalPos = arenaAreasPositions[0];
            for (int i = 0; i < arenaAreasPositions.Length; i++) {

                //controllo che non sia gia occupata
                bool someOneIsNearThisArea = false;
                foreach (IPlayer pd in playerInGame) {
                    if (excludePlayer != pd) {
                        if (Vector3.Distance(((MonoBehaviour)pd).transform.position, arenaAreasPositions[i].position) <2) {
                            //Debug.Log(((MonoBehaviour)excludePlayer).gameObject + " is close to area " + i, ((MonoBehaviour)excludePlayer).gameObject);
                            someOneIsNearThisArea = true;
                            break;
                        }
                    }
                }

                if (someOneIsNearThisArea)
                    continue;

                //se non è occupata la prendo come destinazione
                float curDist = Vector3.Distance(ballPosition.transform.position, arenaAreasPositions[i].position);
                if (curDist> distance) {
                    distance = curDist;
                    finalPos = arenaAreasPositions[i];
                }
                
            }
            return finalPos.position;
        }

        public PlayerData GetClosestPlayerToBall() {
            float distance = Mathf.Infinity;
            PlayerData closestPlayer = null;


            foreach (PlayerData pd in playerInGame) {
                //se non è occupata la prendo come destinazione
                float curDist = Vector3.Distance(pallo.transform.position, pd.transform.position);
                if (curDist < distance) {
                    distance = curDist;
                    closestPlayer = pd;
                }
            }

            return closestPlayer;
        }

        #endregion

    }
}