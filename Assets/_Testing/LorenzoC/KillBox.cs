using System.Collections;
using System.Collections.Generic;
using Controllers;
using LorenzoCastelli;
using UnityEngine;
public class KillBox : MonoBehaviour
{

    public Transform palloResetPos;
    private void OnTriggerEnter(Collider other) {
        PlayerData pd = other.GetComponent<PlayerData>();
        if (pd) {
            Debug.Log("Dead");
            pd.Death();
        } else {
            PalloController pc = other.GetComponent<PalloController>();
            if (pc) {
                pc.gameObject.transform.position = palloResetPos.position;
            }
        }
    }
}
