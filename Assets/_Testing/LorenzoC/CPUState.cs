using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LorenzoCastelli {

public abstract class  CPUState
{
    protected CPUBehaviour owner;
    protected NEWPLAYERSTATES state;
    public Action<NEWPLAYERSTATES> OnEnterState;
    public Action<NEWPLAYERSTATES> OnExitState;

    public NEWPLAYERSTATES State => state;

    public CPUState(CPUBehaviour owner, NEWPLAYERSTATES state) {
        this.owner = owner;
        this.state = state;
    }


    public virtual void Init() {
        OnEnterState?.Invoke(state);
    }
    public abstract void Update();
    public virtual void Exit() {
        OnExitState?.Invoke(state);
    }
}

public enum NEWPLAYERSTATES {
    IDLE,
    GOINGFORBALL,
    CHASING,
    ATTACKING,
    BACKINGOFF,
    RAGDOLL,
    DEATH
}

}