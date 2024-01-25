using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Controllers;
using LorenzoCastelli;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace LorenzoCastelli {

    public class CPUController : PlayerControlsGeneric {

        private static readonly float ORBIT_DISTANCE = 3.5f;

        private bool reachedMyDestination = false;
        public PLAYERSTATES state = PLAYERSTATES.EMPTYHANDED;
        public float maxAttackTime = 4;
        public float currentAttackTime = 0;
        public float maxBackoffTime = 4;
        public float currentBackOffTime = 0;
        private bool closeToTarget = false;
        private bool roamingToLocation = false;
        private Vector3 targetRoamingPos;

        [SerializeField] private float maxChargeTime = 1;
        [SerializeField] private float maxThrowForce = 10;
        private float MinThrowForce => PalloController.SPEED_TIERS[1];

        [SerializeField] public PlayerData playerData;

        [SerializeField] private Rigidbody rb;
        [SerializeField] private float speed = 1;
        [SerializeField] private Transform handsocket;

        public LayerMask collisionLayers;


        private NavMeshAgent ai;

        public GameObject frontPoint;

        public GameObject currentLookTarget;
        public Vector3 currentMoveLocationTarget;
        public float distanceLimit = 0.5f;
        public float turningSpeed = 10f;
        private bool moving = false;
        private Transform areaPos;


        public void ChangeState(PLAYERSTATES newstate) {
            if (newstate != state) {
                state = newstate;
            }
        }


        public override void BallThrow() {
            if (!playerData.IsHoldingBall())
                return;

            if (playerData.throwChargeTime >= maxChargeTime) {
                playerData.GetPallo().Throw(transform.forward * (MinThrowForce + (Mathf.Min(playerData.throwChargeTime, maxChargeTime) * (maxThrowForce - MinThrowForce) / maxChargeTime)) + Vector3.up * 1.2f);
                playerData.LoseBall();
                ChangeState(PLAYERSTATES.EMPTYHANDED);
            } else {
                playerData.throwChargeTime += Time.deltaTime;
            }
        }

        #region MOVEMENT


        public override void PlayerMovement() {

            if (playerData.IsHoldingBall()) {//ho la palla
                if (!currentLookTarget) {
                    Debug.LogError("Non ho un bersaglio; La funzione non assegna currentLookTarget correttamente!");
                    currentMoveLocationTarget = transform.position;
                    return;
                }

                if (closeToTarget) {//sono vicino al target
                    //se sono vicino all'ultima destinazione
                    //calcolo la prossima destinazione come posizione a lato del target (randomicamente alla sua dx o sx)
                    if (Vector3.Distance(transform.position, currentMoveLocationTarget) < ORBIT_DISTANCE / 2) {
                        Vector3 vettore_nemico_me = (currentLookTarget.transform.position - transform.position).normalized;
                        Vector3 side_vector = Vector3.Cross(vettore_nemico_me, Vector3.up);
                        bool leftMovement = Random.Range(0, 100) > 50;
                        currentMoveLocationTarget = leftMovement ?
                            currentLookTarget.transform.position + side_vector :
                            currentLookTarget.transform.position - side_vector;
                    }
                    //transform.RotateAround(currentLookTarget.transform.position, transform.TransformDirection(Vector3.forward + Vector3.right), 0.5f * Time.deltaTime);//non serve
                } else {//sono lontano dal target
                    //se sono lontano dal target, mi ci avvicino
                    currentMoveLocationTarget = currentLookTarget.transform.position;
                }
            }
            else {//non ho la palla
                //currentMoveLocationTarget = currentLookTarget.transform.position;
            }
        }

        private void Roam() {
            //currentLookTarget = GameLogic.instance.FindInterestingPlayer(playerData).gameObject;
            //if (!currentLookTarget) {
                if (!roamingToLocation) {
                Debug.Log("Going here");
                    Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * 0.5f;
                    randomDirection += transform.position;
                    NavMeshHit hit;
                    NavMesh.SamplePosition(randomDirection, out hit, 0.5f, 1);
                    targetRoamingPos = hit.position;
                    currentMoveLocationTarget = targetRoamingPos;
                    ai.SetDestination(targetRoamingPos);
                    roamingToLocation = true;
                } else {
                    if (transform.localPosition == targetRoamingPos) {
                        roamingToLocation = false;
                    }
                }
            /*} else {
                roamingToLocation = false;
            }*/
            

        }

        private void Coinflip() {
            int coin = (int)UnityEngine.Random.Range(0, 10);
            if (coin < 5) {
                ChangeState(PLAYERSTATES.BACKINGOFF);
            } else {
                ChangeState(PLAYERSTATES.ATTACKING);
            }
        }

        

#endregion

        #region ROTATION AND LOOK

        public override void PlayerRotation() {

            if (!GameLogic.instance.pallo.IsHeld)//guarda palla 
                LookTarget(GameLogic.instance.pallo.gameObject);
            else if (currentLookTarget)//guarda target
                LookTarget(currentLookTarget);
        }


        private void LookTarget(GameObject target) {
            if (target) {
                Quaternion _lookRotation = Quaternion.LookRotation((target.transform.position - transform.position).normalized);
                transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * turningSpeed);
                currentLookTarget = target;
            }
        }
        #endregion


        // Start is called before the first frame update
        void Awake() {
            ai = gameObject.GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update() {

            UpdateDestination();//invia ogni tot secondi la destinazione al navmesh

            UpdateAI(); // Controllare se: HoiLpALLO; VadoAPrendereIlPallo; MiNascondoDalPallo
            PlayerMovement();
            PlayerRotation();
            BallThrow();

        }

        bool isAggressive = true;
        float nextAggressiveStateChange = 0;
        void ChangeAggressiveState() {
            if (Time.time> nextAggressiveStateChange) {
                isAggressive = !isAggressive;
                nextAggressiveStateChange = Time.time + Random.Range(3, 6);
            }
        }

        private void UpdateDestination() {

            if (Time.time > nextUpdateDest) {
                ai.SetDestination(currentMoveLocationTarget);//aggiornare questa ogni farme non è il top
                nextUpdateDest = Time.time + .2f;//entra in questo if ogni .5 secondi
            }
        }

        float nextUpdateDest = 0;
        float nextUpdateTarget = 0;

        private void UpdateAI() {

            ChangeAggressiveState();

            if (Time.time> nextUpdateTarget) {
                nextUpdateTarget = Time.time + 1;
                currentLookTarget = GameLogic.instance.FindInterestingPlayer(playerData).gameObject;
            }


            if (playerData.IsHoldingBall()) {//IO HO IL PALLO

                //check close to target
                if (Vector3.Distance(currentLookTarget.transform.position, this.transform.position) <= ORBIT_DISTANCE) {
                    closeToTarget = true;
                    Debug.Log(this.gameObject + "orbito target");
                } else {
                    closeToTarget = false;
                    Debug.Log(this.gameObject + "vado per il target");
                }
            }
            else if (GameLogic.instance.pallo.IsHeld) {//QUALCUN ALTRO HA LA PALLA

                if (isAggressive) {
                    //se sono aggressivo vado a priori verso il giocatore con la palla
                    currentMoveLocationTarget = GameLogic.instance.pallo.gameObject.transform.position;
                }
                else {
                    Vector3 farrestArea = GameLogic.instance.FindFarestPointV2(GameLogic.instance.pallo.gameObject, playerData);
                    currentMoveLocationTarget = farrestArea;
                }
                
            } else {//NESSUNO HA LA PALLA

                if (isAggressive) {
                    //se sono aggressivo vado a priori verso la palla
                    currentMoveLocationTarget = GameLogic.instance.pallo.gameObject.transform.position;
                } else {
                    PlayerData closestPlayerToBall = GameLogic.instance.GetClosestPlayerToBall();

                    if (playerData == closestPlayerToBall)//io sono il piu vicino
                    {
                        currentMoveLocationTarget = GameLogic.instance.pallo.transform.position;
                    } else {//scappo / sono corragioso
                        Vector3 farrestArea = GameLogic.instance.FindFarestPointV2(GameLogic.instance.pallo.gameObject, playerData);
                        currentMoveLocationTarget = farrestArea;
                    }
                }
            }
        }




        private void OnTriggerEnter(Collider other) {
            if ((other.GetComponent<PalloController>() != null) && (!other.GetComponent<PalloController>().IsHeld)) {
                ChangeState(PLAYERSTATES.WITHBALL);
                playerData.PickUpBall(other.GetComponent<PalloController>());
                playerData.GetPallo().Hold(handsocket);
                currentLookTarget = GameLogic.instance.FindInterestingPlayer(playerData).gameObject;
            }
        }
    }

    public enum PLAYERSTATES {
        EMPTYHANDED,
        GOINGFORBALL,
        WITHBALL,
        BACKINGOFF,
        ATTACKING,
        RAGDOLL,
        DEAD
    }

}