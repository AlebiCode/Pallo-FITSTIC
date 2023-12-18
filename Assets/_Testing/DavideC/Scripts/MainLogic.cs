using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainLogic : MonoBehaviour
{
    public static MainLogic instance;

    [SerializeField] private GameObject playerModel;
    [SerializeField] private Vector3 spawnLocation = Vector3.zero;
    [SerializeField] private Transform playerContainer;

    private void Awake()
    {
        if (instance)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

    }

    public void SpawnPlayer()
    {
        Instantiate(playerModel, spawnLocation, Quaternion.Euler(0, 0, 0), playerContainer);
    }
}
