using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : MonoBehaviour
{
    [SerializeField] private GameObject[] traps;
    [SerializeField] private int activeTraps;

    private void Awake()
    {
        for (int i = 0; i < traps.Length; i++)
        {
            traps[i].SetActive(false);
        }
    }

    private void Start()
    {
        for (int i = 0; i == activeTraps; i++)
        {
            var trapId = Random.Range(0, traps.Length);
            traps[trapId].SetActive(true);
        }
    }
}
