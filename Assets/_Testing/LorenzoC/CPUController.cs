using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using LorenzoCastelli;
using UnityEngine;
using UnityEngine.AI;

namespace LorenzoCastelli {

    public class CPUController : PlayerControlsGeneric {


        public PLAYERSTATES state = PLAYERSTATES.EMPTYHANDED;
        private float maxAttackTime = 10;
        private float currentAttackTime = 0;
        private float maxBackoffTime = 10;
        private float currentBackOffTime = 0;


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
        public float distanceLimit = 6.66f;
        public float turningSpeed = 10f;





        public override void BallThrow() {
            if (!playerData.IsHoldingBall())
                return;

            if (playerData.throwChargeTime >= maxChargeTime) {
                playerData.GetPallo().Throw(transform.forward * (MinThrowForce + (Mathf.Min(playerData.throwChargeTime, maxChargeTime) * (maxThrowForce - MinThrowForce) / maxChargeTime)) + Vector3.up * 1.2f);
                playerData.LoseBall();
                state = PLAYERSTATES.EMPTYHANDED;
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
                    currentMoveLocationTarget = GameLogic.instance.pallo.transform.position;
                    ai.SetDestination(currentMoveLocationTarget);
                    break;
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
                    if (GameLogic.instance.GetClosestMostImportantPlayer(playerData)) {
                        //se sono vicino ad uno con la palla decido cosa fare
                        //Coinflip();
                    } else {
                        state = PLAYERSTATES.GOINGFORBALL;
                    }
                    break;

                default:
                    break;
            }
        }

        private void AttackingState() {
            if (currentLookTarget) {
                if (Vector3.Distance(currentLookTarget.transform.position, transform.position) <= 0.2f) {
                    //Se è vicino triggera attacco
                    //ATTACCO 
                    currentBackOffTime = 0;
                    state = PLAYERSTATES.EMPTYHANDED;
                }
            }
            currentAttackTime += Time.deltaTime;
            if (currentAttackTime >= maxAttackTime) {
                currentAttackTime = 0;
                state = PLAYERSTATES.EMPTYHANDED;
            }
        }

        public void BackingOff() {
            if (currentLookTarget) {
                // Calculate the direction from this GameObject to the target
                Vector3 direction = transform.position - currentLookTarget.transform.position;

                // Normalize the direction to get a unit vector
                direction.Normalize();

                ai.SetDestination(direction*1.5f);
                currentMoveLocationTarget = direction* 1.5f;
                currentBackOffTime += Time.deltaTime;
                if (currentBackOffTime <= maxBackoffTime) {
                    currentBackOffTime = 0;
                    state = PLAYERSTATES.EMPTYHANDED;
                }
            } else {
                currentBackOffTime = 0;
                state = PLAYERSTATES.EMPTYHANDED;
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
                state = PLAYERSTATES.WITHBALL;
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