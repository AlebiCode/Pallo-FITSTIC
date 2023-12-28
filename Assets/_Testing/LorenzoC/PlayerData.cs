using Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LorenzoCastelli {

public class PlayerData : MonoBehaviour
{

        public float lookDistance = 50f;
        private int maxHp=100;
        public int currentHp;
        //public int speed;
        public int ragdollTreshold;


        public float throwChargeTime = 0;

        private PalloController heldPallo;

        public int importance;


        private void Awake() {
            currentHp = maxHp;
        }

        public void TakeDamage(int amount) {
            currentHp -= amount;
            if (currentHp <= 0) {
                Death();
            } else {
            if (amount >= ragdollTreshold) {
                    //RAGDOLLA
                } else {
                    //ANIMAZIONE DI HIT
                }

            }
        }

        public void ForceRagdoll() {

        }



        private void Death() {
            currentHp = 0;
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
                GameLogic.instance.ForceDistanceBots(this);
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

        public bool IsHoldingBall() {
            return heldPallo;
        }

        internal void CheckInterest() {
            throw new NotImplementedException();
        }
    }

}