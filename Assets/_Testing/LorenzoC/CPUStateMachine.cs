using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LorenzoCastelli {

public class CPUStateMachine : MonoBehaviour
{
        private GameLogic instance;
        private Dictionary<NEWPLAYERSTATES, CPUState> stateDictionary;
        private CPUState currentState;
        private CPUBehaviour owner;

        #region Context Variables
        private Vector3 lastPositionOnPath;
        private Vector3 targetNode;
        private GameObject aggroedObject;
        private bool targetIsPlayer;
        #endregion

        #region Get/Set
        public Vector3 LastTargetPosition { get { return lastPositionOnPath; } set { lastPositionOnPath = value; } }
        public Vector3 TargetPosition { get { return targetNode; } set { targetNode = value; } }
        public GameObject CurrentTarget { get { return aggroedObject; } set { aggroedObject = value; } }

        public bool IsTargetPlayer {get { return targetIsPlayer; } set { targetIsPlayer = value; } }

        public GameLogic Instance { get { return instance; } }
        public Dictionary<NEWPLAYERSTATES, CPUState> StateDictionary { get => stateDictionary; private set => stateDictionary = value; }
        #endregion


        public CPUStateMachine(CPUBehaviour owner, Dictionary<NEWPLAYERSTATES, CPUState> stateDictionary) {
            this.owner = owner;
            this.StateDictionary = stateDictionary;
            //temp
            CurrentTarget = owner.player.gameObject;
        }

        public void ChangeState(NEWPLAYERSTATES state) {



            currentState?.Exit();
            currentState = StateDictionary[state];
            owner.State = state;
            currentState.Init();

        }



        public void UpdateState() {
            currentState?.Update();
        }


        public void DecideAction() {
            int coin = Random.Range(0, 9);
            if (coin <= 4) {
                currentState = StateDictionary[NEWPLAYERSTATES.BACKINGOFF];
                owner.State = NEWPLAYERSTATES.BACKINGOFF;
            } else {
                currentState = StateDictionary[NEWPLAYERSTATES.ATTACKING];
                owner.State = NEWPLAYERSTATES.ATTACKING;
            }
            currentState.Init();
        }

        /*public Vector3 GetClosestNode() {

           // Vector3 closestNode = owner.PatrolNodes[0];
            float nodeDistance;
            float closestNodeDistance = Vector3.Distance(owner.transform.position, closestNode);


            /*foreach (Vector3 node in owner.PatrolNodes) {
                nodeDistance = Vector3.Distance(owner.transform.position, node);
                if (nodeDistance < closestNodeDistance) {
                    closestNode = node;
                    closestNodeDistance = nodeDistance;
                }
            }

            return closestNode;
        //}*/

    }






}


