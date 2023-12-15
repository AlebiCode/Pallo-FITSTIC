using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class testabs : MonoBehaviour
{

    protected abstract void TakeDamage(float dmg);

    protected virtual void TakeLol(float lol)
    {
        Debug.Log("Lool");
    }

}

public class LOLOL : testabs
{
    private void Update()
    {
        
    }

    protected override void TakeDamage(float dmg)
    {
        throw new System.NotImplementedException();
    }
    protected override void TakeLol(float lol)
    {
        base.TakeLol(lol);
        //awdawdawfawd
    }

}