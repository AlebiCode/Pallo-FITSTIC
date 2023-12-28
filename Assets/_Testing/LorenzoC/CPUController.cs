using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using LorenzoCastelli;
using UnityEngine;
using UnityEngine.AI;

namespace LorenzoCastelli {

    public class CPUController : PlayerControlsGeneric {


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


        public void MoveTo(Vector3 position) {
            ai.SetDestination(position);
        }


        public override void BallThrow() {
            if (!playerData.IsHoldingBall())
                return;

            if (playerData.throwChargeTime >= maxChargeTime) {
                playerData.GetPallo().Throw(transform.forward * (MinThrowForce + (Mathf.Min(playerData.throwChargeTime, maxChargeTime) * (maxThrowForce - MinThrowForce) / maxChargeTime)) + Vector3.up * 1.2f);
                playerData.LoseBall();
            } else {
                playerData.throwChargeTime += Time.deltaTime;
            }
        }

        public override void PlayerMovement() {
            if (playerData.IsHoldingBall()) {
                if (!currentLookTarget.GetComponent<PalloController>()) {
                    //
                    if (Vector3.Distance(currentLookTarget.transform.position, this.transform.position) <= 0.5f) {
                        transform.RotateAround(currentLookTarget.transform.position, transform.TransformDirection(Vector3.forward+Vector3.right), 0.5f * Time.deltaTime);
                    } else {
                        currentMoveLocationTarget = currentLookTarget.transform.position;
                        ai.SetDestination(currentMoveLocationTarget);
                    }

                } else {
                    currentMoveLocationTarget = GameLogic.instance.FindInterestingPlayer(this.playerData).gameObject.transform.position;
                }
            } else {
                //CONTROLLO SE LA PALLA L'HA PRESO QUALCUNO OPPURE NO
                currentMoveLocationTarget = GameLogic.instance.pallo.transform.position;
                ai.SetDestination(currentMoveLocationTarget);
            }
        }

        private void LookForTarget() {
                //SE NON STO GUARDANDO NESSUNO CERCA UN NUOVO TARGET
                //PRIORITà AI PLAYER CON LA PALLA/IMPORTANZA
                int maxImportance = 0;
                //List<PlayerData> playerList = new List<PlayerData>();
               // playerList = instance.playerInGame;
                foreach (PlayerData player in GameLogic.instance.playerInGame) {
                    if ((player.importance > maxImportance) && (Vector3.Distance(player.gameObject.transform.position, this.transform.position) <= distanceLimit) && (player != gameObject.GetComponent<PlayerData>())) {
                        currentLookTarget = player.gameObject;
                        maxImportance = player.importance;
                    }
                }
                if (maxImportance <= 0) {
                    if ((Vector3.Distance(GameLogic.instance.pallo.gameObject.transform.position, this.transform.position) <= distanceLimit) && (!playerData.IsHoldingBall())) {
                        currentLookTarget = GameLogic.instance.pallo.gameObject;
                }
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
                playerData.PickUpBall(other.GetComponent<PalloController>());
                playerData.GetPallo().Hold(handsocket);
                currentLookTarget = GameLogic.instance.FindInterestingPlayer(playerData).gameObject;


                /*if (playerData.IsHoldingBall()) {
                    throwChargeTime = 0;
                    heldPallo.Hold(handsocket);
                }*/


            }


        }
    }

    public enum PLAYERSTATES {
        EMPTYHANDED,
        WITHBALL,
        RAGDOLL,
        DEAD
    }

}