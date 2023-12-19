using Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LorenzoCastelli {


public class PlayerData : MonoBehaviour
{
        private int maxHp=100;
        public int currentHp;
        //public int speed;
        public int ragdollTreshold;

        // Start is called before the first frame update
        private void Start() {
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
    }

}