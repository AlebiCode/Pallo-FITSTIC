using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LorenzoCastelli {

    public class CPU_Idle : CPUState {
        public CPU_Idle(CPUBehaviour owner) : base(owner, NEWPLAYERSTATES.IDLE) {

        }
        public override void Update() { }

    }
}