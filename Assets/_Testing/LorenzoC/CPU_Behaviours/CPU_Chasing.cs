using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LorenzoCastelli {

public class CPU_Chasing : CPUState {
        GameObject targetObject;
        float maxChasingTime = 10;
        float currentChasingTime;

    public CPU_Chasing(CPUBehaviour owner) : base(owner, NEWPLAYERSTATES.CHASING) {
    }


        public override void Update() {
            maxChasingTime += (int)Time.deltaTime;
            if (maxChasingTime > 10) {
                owner.stateMachine.DecideAction();
                return;
            } else {
                if ((!GameLogic.instance.IsPlayerClose(owner.GetComponent<PlayerData>(), targetObject.GetComponent<PlayerData>()) || (!targetObject.GetComponent<PlayerData>().IsHoldingBall()))) {
                    //Se è ancora vicino al nemico con la palla fai il coin flip
                    owner.stateMachine.ChangeState(NEWPLAYERSTATES.IDLE);
                    return;
                } else {
                    if (Vector3.Distance(targetObject.transform.position, owner.transform.position) <= 0.5f) {
                        //attacco
                        owner.stateMachine.DecideAction();
                    } else {
                        owner.Ai.SetDestination(targetObject.transform.position);
                    }
                }
            }
        }

    }

}