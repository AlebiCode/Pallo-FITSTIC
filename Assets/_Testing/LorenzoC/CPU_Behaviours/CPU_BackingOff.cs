using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LorenzoCastelli {


    public class CPU_BackingOff : CPUState {

        GameObject targetObject;
        float maxBackingOffTime = 5;
        float currentBackingOffTime;

        public CPU_BackingOff(CPUBehaviour owner) : base(owner, NEWPLAYERSTATES.BACKINGOFF) {
        }

        public override void Exit() {
            base.Exit();
        }

        public override void Init() {

            base.Init();
            currentBackingOffTime = 0;
            //owner.OnAttackFinished += EndAttack;
            owner.transform.LookAt(owner.stateMachine.CurrentTarget.transform.position);
        }


        public override void Update() {

            if ((targetObject) && (targetObject.GetComponent<PlayerData>().IsHoldingBall())) {
                // Calculate the direction from this GameObject to the target
                /*Vector3 direction = transform.position - targetObject.transform.position;

                // Normalize the direction to get a unit vector
                direction.Normalize();

                ai.SetDestination(direction * 1.5f);
                currentMoveLocationTarget = direction * 1.5f;
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
                */

            }
        }

    }
}