using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayer
{
    public bool IsAlive { get; }

    public int CurrentHp { get; }
    public int Importance => 0; //TODO

}
