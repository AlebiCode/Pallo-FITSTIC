using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

namespace LorenzoCastelli { 
public abstract class PlayerControlsGeneric : MonoBehaviour
{
    public abstract void PlayerMovement();
    public abstract void PlayerRotation();

    public abstract void BallThrow();
    }
}
