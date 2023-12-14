using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LorenzoCastelli
{

public class PlayerControll: PlayerControlsGeneric {
        

        [SerializeField] private float maxChargeTime = 1;
        [SerializeField] private float maxThrowForce = 10;

        [SerializeField] public PlayerData playerData;
        
        [SerializeField] private Rigidbody rb;
        [SerializeField] private float speed = 1;
        [SerializeField] private Transform handsocket;
        //[SerializeField] private float minThrowForce = 1;

        private Vector2 directionInput;
        private PalloController heldPallo;
        private float throwChargeTime = 0;

        private bool IsHoldingBall => heldPallo;
        private float MinThrowForce => PalloController.TIER_2_SPEED;

        private void Update() {
            PlayerMovement();
            PlayerRotation();
            BallThrow();
        }


        public override void PlayerMovement() {
            PlayerInputsMovement();
        }

        private void PlayerInputsMovement() {
            directionInput.x = Input.GetAxis("Horizontal");
            directionInput.y = Input.GetAxis("Vertical");

            transform.position += new Vector3(directionInput.x, 0, directionInput.y) * speed * Time.deltaTime;
        }

        public override void PlayerRotation() {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000)) {
                transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z), Vector3.up);
            }
    }
        public override void BallThrow(){
            if (!IsHoldingBall)
                return;

            if (throwChargeTime >= maxChargeTime || Input.GetKeyUp(KeyCode.Mouse0)) {
                heldPallo.Throw(transform.forward * (MinThrowForce + (Mathf.Min(throwChargeTime, maxChargeTime) * (maxThrowForce - MinThrowForce) / maxChargeTime)) + Vector3.up * 1.2f);
                heldPallo = null;
            } else if (Input.GetKey(KeyCode.Mouse0)) {
                throwChargeTime += Time.deltaTime;
            }
        }


        private void OnTriggerEnter(Collider other) {
            if (other.GetComponent<PalloController>() != null) {
                heldPallo = other.GetComponent<PalloController>();
                if (heldPallo != null) {
                    if (IsHoldingBall) {
                        throwChargeTime = 0;
                        heldPallo.Hold(handsocket);
                    }

                }
            }
        }

    }


}