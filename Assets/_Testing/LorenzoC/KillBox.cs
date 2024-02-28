using System.Collections;
using System.Collections.Generic;
using Controllers;
using LorenzoCastelli;
using UnityEngine;
public class KillBox : MonoBehaviour
{
    public bool shouldResetDirection = false;
    public Transform palloResetPos;
    private void OnTriggerEnter(Collider other) {
        PlayerData pd = other.GetComponent<PlayerData>();
        if (pd) {
            if (pd.IsHoldingBall()) {
                pd.LoseBall();
            }
            Debug.Log("Dead");
            pd.Death();
        } else {
            PalloController pc = other.GetComponent<PalloController>();
            if (pc) {
                if (shouldResetDirection) {
                pc.BallState = PalloController.BallStates.respawning;
                }
                pc.gameObject.transform.position = palloResetPos.position;
            }
        }
    }
}
