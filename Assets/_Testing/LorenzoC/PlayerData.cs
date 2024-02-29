using Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using vittorio;

namespace LorenzoCastelli {

public class PlayerData : MonoBehaviour
{
        [Header("General Data")]
        public int maxHp = 100; 
        private int currentHp;
        public Transform handsocket;
        private PalloController heldPallo;
        public LayerMask palloLayermask;
        public PalloTrigger palloTrigger;


        [Header("Ambient interactions")]
        //[SerializeField] public PalloTrigger parryHitbox;
        public bool playerIsBeingRejected = false;

        [Header("Movement")]
        public float speedRegular = 5f;


        [Header("Throw")]
        public float speedSlow = 2f;
        public float throwChargeMax = 1f;
        public float holdBallCooldown = 2f;
        private float throwChargeCurrent = 0f;
        private float throwForceMax = 10f;
        private float throwHeight = 1.2f;
        private float holdBallCooldownCurrent = 0f;

        [Header("Dodge")]
        public float speedDodge = 15f;
        public float dodgeDuration = .6f; //durata dodge
        public float dodgeCooldown = 1f; //durata dodge cooldown
        public float dodgeDecrease = 2f; //quanto velocemente diminuisce la velocità di dodge
        private float dodgeCooldownCurrent = 0f;

        [Header("Parry")]
        public float parryDuration = 0.5f; //quanto a lungo dura l'azione del parry
        public float parryPercentage = 60f; //percentuale del parry in cui sei invincibile
        public float parryRange = 2f; //quanto lontano raggiunge il parry
        public float parryCooldown = 1f;
        public bool isInvincibile = false;
        private float parryCooldownCurrent = 0f;

        [Header("Stun")]
        public float pushFactor = 2f;
        public float pushDecrease = 2f;
        private Vector3 pushDirection;
        private float pushForce;

        //properties
        public PalloController HeldPallo { get => heldPallo; set => heldPallo = value; }
        public Transform Handsocket { get => handsocket; set => handsocket = value; }
        public float DodgeCooldownCurrent { get => dodgeCooldownCurrent; set => dodgeCooldownCurrent = value; }
        public float ParryCooldownCurrent { get => parryCooldownCurrent; set => parryCooldownCurrent = value; }
        public float HoldBallCooldownCurrent { get => holdBallCooldownCurrent; set => holdBallCooldownCurrent = value; }
        public float ThrowChargeCurrent { get => throwChargeCurrent; set => throwChargeCurrent = value; }
        public bool IsInvincibile { get => isInvincibile; set => isInvincibile = value; }
        public Vector3 PushDirection { get => pushDirection; set => pushDirection = value; }
        public float PushForce { get => pushForce; set => pushForce = value; }
        public float PushDecrease { get => pushDecrease; set => pushDecrease = value; }
        public float ThrowForceMin => PalloController.SPEED_TIERS[1];
        public bool IsHoldingBall => heldPallo;


        public PlayerAnimation playerAnimation;
        public float lookDistance = 0.5f;
        public Rigidbody rb;
        //public int speed;
        public int ragdollTreshold;


        public float throwChargeTime = 0;


        public int importance;

        private void Awake() {
            currentHp = maxHp;
        }

        private void Start() {
            palloTrigger.AddOnEnterListener(PalloContact);
            //parryHitbox.AddOnEnterListener(GrabPallo);
        }

        private void Update() {

            HandleCooldowns();
        }

        public Vector3 ThrowVelocity {
            get {
                return transform.forward *
                (ThrowForceMin + (Mathf.Min(throwChargeCurrent, throwChargeMax)
                * (throwForceMax - ThrowForceMin) / throwChargeMax))
                + Vector3.up * throwHeight;
            }
        }

        public int CurrentHp {
            get { return currentHp; }

            set {
                if (value < 0)
                    currentHp = 0;
                else
                    currentHp = value;
            }
        }


        private void HandleCooldowns() {
            //dodge cooldown
            if (DodgeCooldownCurrent > 0) {
                DodgeCooldownCurrent -= Time.deltaTime;
            }

            //parry cooldown
            if (ParryCooldownCurrent > 0) {
                ParryCooldownCurrent -= Time.deltaTime;
            }


            //hold ball cooldown
            if ((HoldBallCooldownCurrent > 0) && (!IsHoldingBall)) {
                HoldBallCooldownCurrent -= Time.deltaTime;
            }
        }

        private void PalloContact(PalloController pallo) {
            if (pallo.BallState == PalloController.BallStates.thrown)
                TakeDamage(10);
            else
                GrabPallo(pallo);
            /*switch (pallo.BallState) {
                case PalloController.BallStates.thrown:

                    GetPushed(
                        pallo.Velocity.normalized,
                        PalloController.SPEED_TIERS[pallo.SpeedTier] * pushFactor);

                    break;

                case PalloController.BallStates.bouncing:

                    if (holdBallCooldown <= 0) {
                        heldPallo = pallo;
                        heldPallo.Hold(Handsocket);
                    }

                    break;
            }*/
        }

        private void GetPushed(Vector3 pushDirection, float pushForce) {
            if (!IsInvincibile) {
                PushDirection = pushDirection;
                PushForce = pushForce;
            }
        }

        public void TakeDamage(int amount) {
            CurrentHp -= amount;

            Debug.Log("Player HP = " + CurrentHp);

            if (CurrentHp <= 0) {
                this.KillPlayer();
            }
        }

        public void KillPlayer() {
            //TODO trasformare in unityEvent

            Debug.Log("player is killed!");
            //TODO rimuovi destroy e togli una vita
            Destroy(this.gameObject);
        }






        /*public void TakeDamage(int amount) {
            currentHp -= amount;
            if (currentHp <= 0) {
                playerAnimation.PlayAnimation(PlayerAnimation.death);
                Death();
            } else {
            if (amount >= ragdollTreshold) {
                    //RAGDOLLA
                } else {
                    playerAnimation.PlayAnimation(PlayerAnimation.hit);
                    //ANIMAZIONE DI HIT
                }

            }
        }*/

        public void ForceRagdoll() {

        }



        public void Death() {
            currentHp = 0;
            GetComponent<Rigidbody>().useGravity = true;
            GameLogic.instance.RemovePlayer(this);
            Destroy(this.gameObject);
            //UPDATE DELLA UI
            //RAGDOLL
        }

        public bool isAlive() {
            if (currentHp > 0)
                return true;
            else
                return false;
        }

        public void AddImportance(int value) {
            importance += value;
        }

        public void PickUpBall(PalloController pallo) {
            if (pallo) {
                heldPallo = pallo;
                throwChargeTime = 0;
                importance += 10;
                GameLogic.instance.ForceLookTarget(this);
                pallo.PlayerHoldingIt = this;
                heldPallo.Hold(handsocket);
            } else {
                Debug.LogError("Couldn't find " + pallo);
            }
        }

        public PalloController GetPallo() {
            return heldPallo;
        }

        public void LoseBall() {
            heldPallo = null;
            importance -= 10;
            GameLogic.instance.ForceLookTarget();
        }


        internal void CheckInterest() {
            throw new NotImplementedException();
        }
        public void GrabPallo(PalloController palloController) {
            heldPallo = palloController;
            throwChargeTime = 0;
            heldPallo.Hold(handsocket);
            palloController.playerHolding = this;
        }

        private float velocityChange = 0;
        RaycastHit castInfo;
        private void PlayerAirborneControl() {
            Physics.SphereCast(transform.position, GetComponent<Collider>().bounds.extents.x, Vector3.down, out castInfo, transform.position.y - (GetComponent<Collider>().bounds.extents.y + 0.1f), 1 << 0);

            if (playerIsBeingRejected) {
                if (castInfo.collider) {
                    StopAirbornePlayer();

                    Debug.Log("player stopped");
                } else {
                    SlowPlayerVerticalSpeed();

                    Debug.Log("player being slowed");
                }
            }
        }
        public void SlowPlayerVerticalSpeed() {
            velocityChange += 0.5f * Time.deltaTime;
            rb.velocity = new Vector3(rb.velocity.x, -velocityChange, rb.velocity.z);
        }
        public void StopAirbornePlayer() {
            playerIsBeingRejected = false;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
        public void SetPlayerToRejectState() {
            playerIsBeingRejected = true;
            velocityChange = 0f;
        }

        private void FixedUpdate() {
            PlayerAirborneControl();
        }
    }


}


