using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LorenzoCastelli {


public class CPU_BackingOff : CPUState {
        public CPU_BackingOff(CPUBehaviour owner) : base(owner, NEWPLAYERSTATES.BACKINGOFF) {
        }

        public override void Update() { }
    }

}