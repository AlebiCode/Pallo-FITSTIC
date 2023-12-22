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

        [SerializeField] public PlayerData playerData;

        [SerializeField] private Rigidbody rb;
        [SerializeField] private float speed = 1;
        [SerializeField] private Transform handsocket;

        public LayerMask collisionLayers;


        private NavMeshAgent ai = new NavMeshAgent();

        public GameObject frontPoint;

        public GameObject currentTarget;
        public float distanceLimit = 6.66f;
        public float turningSpeed = 10f;

        public override void BallThrow() {
            throw new System.NotImplementedException();
        }

        public override void PlayerMovement() {
            if (playerData.IsHoldingBall()) {
                if (currentTarget) {
                    transform.RotateAround(currentTarget.transform.position, transform.TransformDirection(Vector3.forward), 2f * Time.deltaTime);

                }
                //SCEGLI UN BERSAGLIO OPPURE MUOVITI DA QUALCHE PARTE

            } else {
                //CONTROLLO SE LA PALLA L'HA PRESO QUALCUNO OPPURE NO
                ai.SetDestination(GameObject.FindGameObjectWithTag("Pallo").transform.position);
            }
        }

        private void LookForTarget() {
                //SE NON STO GUARDANDO NESSUNO CERCA UN NUOVO TARGET
                //PRIORITà AI PLAYER CON LA PALLA/IMPORTANZA
                int maxImportance = 0;
                //List<PlayerData> playerList = new List<PlayerData>();
               // playerList = instance.playerInGame;
                foreach (PlayerData player in GameLogic.instance.playerInGame) {
                    if ((player.importance > maxImportance) && (Vector3.Distance(player.gameObject.transform.position, this.transform.position) <= distanceLimit)) {
                        currentTarget = player.gameObject;
                        maxImportance = player.importance;
                    }
                }
                if (maxImportance <= 0) {
                    if (Vector3.Distance(GameLogic.instance.pallo.gameObject.transform.position, this.transform.position) <= distanceLimit) {
                        currentTarget = GameLogic.instance.pallo.gameObject;
                }
            }
        }

        public override void PlayerRotation() {
            if (!currentTarget) {
                LookForTarget();
            } else {
                if (Vector3.Distance(currentTarget.transform.position, this.transform.position) > distanceLimit) {
                    currentTarget = null;
                } else {
                    LookTarget(currentTarget);
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

        }

        private void OnTriggerEnter(Collider other) {
            if ((other.GetComponent<PalloController>() != null) && (!other.GetComponent<PalloController>().IsHeld)) {
                playerData.PickUpBall(other.GetComponent<PalloController>());
                playerData.GetPallo().Hold(handsocket);
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