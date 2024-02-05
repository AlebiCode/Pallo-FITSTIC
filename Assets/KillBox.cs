using System.Collections;
using System.Collections.Generic;
using LorenzoCastelli;
using UnityEngine;
public class KillBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        PlayerData pd = other.GetComponent<PlayerData>();
        if (pd) {
            Debug.Log("Dead");
            pd.Death();
        }
    }
}
