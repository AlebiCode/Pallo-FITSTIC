using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LorenzoCastelli {

public class CPU_Death : CPUState {
        public CPU_Death(CPUBehaviour owner) : base(owner, NEWPLAYERSTATES.DEATH) {
        }


        public override void Update() { }
    }

}