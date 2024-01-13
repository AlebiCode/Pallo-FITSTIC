using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Controllers;
using LorenzoCastelli;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace LorenzoCastelli {

    public class CPUController : PlayerControlsGeneric {


        public PLAYERSTATES state = PLAYERSTATES.EMPTYHANDED;
        public float maxAttackTime = 4;
        public float currentAttackTime = 0;
        public float maxBackoffTime = 4;
        public float currentBackOffTime = 0;
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


        private NavMeshAgent ai = new NavMeshAgent();

        public GameObject frontPoint;

        public GameObject currentLookTarget;
        public Vector3 currentMoveLocationTarget;
        public float distanceLimit = 0.5f;
        public float turningSpeed = 10f;


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
            switch (state) {
                case PLAYERSTATES.BACKINGOFF:
                    BackingOff();
                    break;
                case PLAYERSTATES.ATTACKING:
                    AttackingState();
                    break;
                case PLAYERSTATES.GOINGFORBALL:
                    if (!GameLogic.instance.pallo.IsHeld) {
                        currentMoveLocationTarget = GameLogic.instance.pallo.transform.position;
                        ai.SetDestination(currentMoveLocationTarget);
                        break;
                    } else {
                        ChangeState(PLAYERSTATES.EMPTYHANDED);
                        break;
                    }
                    
                case PLAYERSTATES.WITHBALL:
                    if (!currentLookTarget.GetComponent<PalloController>()) {
                        //
                        if (Vector3.Distance(currentLookTarget.transform.position, this.transform.position) <= 0.5f) {
                            transform.RotateAround(currentLookTarget.transform.position, transform.TransformDirection(Vector3.forward + Vector3.right), 0.5f * Time.deltaTime);
                        } else {
                            currentMoveLocationTarget = currentLookTarget.transform.position;
                            ai.SetDestination(currentMoveLocationTarget);
                        }

                    } else {
                        currentMoveLocationTarget = GameLogic.instance.FindInterestingPlayer(this.playerData).gameObject.transform.position;
                    }
                    break;
                case PLAYERSTATES.EMPTYHANDED:
                    if ((GameLogic.instance.pallo.IsHeld) && (!currentLookTarget)) {
                        if (GameLogic.instance.GetClosestMostImportantPlayer(playerData)) {
                            //se sono vicino ad uno con la palla decido cosa fare
                            LookTarget(GameLogic.instance.GetClosestMostImportantPlayer(playerData).gameObject);
                            Coinflip();
                            break;
                        } else {
                            Roam();
                            break;
                        }
                    } else if (!GameLogic.instance.pallo.IsHeld) {
                        ChangeState(PLAYERSTATES.GOINGFORBALL);
                        break;
                    } else {
                        Roam();
                        break;
                    }

                default:
                    break;
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

        private void AttackingState() {
            if ((currentLookTarget)&& (currentLookTarget.GetComponent<PlayerData>().IsHoldingBall())) {
               // if (Vector3.Distance(currentLookTarget.transform.position, transform.position) <= 0.5f) {
                //Se è vicino ma non abbastanza vicino raggiungilo
                //ATTACCO 
                currentMoveLocationTarget=currentLookTarget.transform.position;
                    ai.SetDestination(currentMoveLocationTarget);
                    currentAttackTime += Time.deltaTime;
                    if (currentAttackTime >= maxAttackTime) {
                        currentAttackTime = 0;
                        ChangeState(PLAYERSTATES.EMPTYHANDED);
                        return;
                    }
                if (Vector3.Distance(currentLookTarget.transform.position, transform.position) <= 0.3f) {
                    float temp = Vector3.Distance(currentLookTarget.transform.position, transform.position);
                        Debug.Log("Player " + gameObject.name + " Is attacking " + currentLookTarget.name);
                        currentAttackTime = 0;
                        ChangeState(PLAYERSTATES.EMPTYHANDED);
                        return;
                    }

           // }
            } else {
                currentAttackTime = 0;
                ChangeState(PLAYERSTATES.EMPTYHANDED);
                return;
            }
            
        }

        public void BackingOff() {
            if ((currentLookTarget) && (currentLookTarget.GetComponent<PlayerData>().IsHoldingBall())) {
                // Calculate the direction from this GameObject to the target
                Vector3 direction = transform.position - currentLookTarget.transform.position;

                // Normalize the direction to get a unit vector
                direction.Normalize();

                ai.SetDestination(direction*1.5f);
                currentMoveLocationTarget = direction* 1.5f;
                currentBackOffTime += Time.deltaTime;
                if (currentBackOffTime > maxBackoffTime) {
                    currentBackOffTime = 0;
                    ChangeState(PLAYERSTATES.EMPTYHANDED);
                    return;
                }
            } else {
                currentBackOffTime = 0;
                ChangeState(PLAYERSTATES.EMPTYHANDED);
                return;
            }
            
        }

        public void MoveTo(Vector3 position) {
            ai.SetDestination(position);
        }

#endregion

        #region ROTATION AND LOOK
        private void LookForTarget() {
            //SE NON STO GUARDANDO NESSUNO CERCA UN NUOVO TARGET

            PlayerData target = GameLogic.instance.GetClosestMostImportantPlayer(playerData);
                //PRIORITà AI PLAYER CON LA PALLA/IMPORTANZA
                if (!target) {
                    if ((Vector3.Distance(GameLogic.instance.pallo.gameObject.transform.position, this.transform.position) <= distanceLimit) && (!playerData.IsHoldingBall())) {
                        currentLookTarget = GameLogic.instance.pallo.gameObject;
                }
            } else {
                currentLookTarget = target.gameObject;
            }
        }

        public override void PlayerRotation() {
            if (!currentLookTarget) {
                //NON HO UN BERSAGLIO
                if (playerData.IsHoldingBall() && currentLookTarget.GetComponent<PalloController>()) {
                    //STO TENENDO LA PALLA
                    currentLookTarget = GameLogic.instance.FindInterestingPlayer(this.GetComponent<PlayerData>()).gameObject;
                } else {
                    LookForTarget();
                }
                //if (gameObject.GetComponent<PlayerData>())
            } else {
                if (Vector3.Distance(currentLookTarget.transform.position, this.transform.position) > distanceLimit) /*|| (currentTarget.GetComponent<PalloController>().IsHeld))*/ {
                    currentLookTarget = null;
                } else {
                    LookTarget(currentLookTarget);
                }
            }
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
            PlayerMovement();
            PlayerRotation();
            BallThrow();
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