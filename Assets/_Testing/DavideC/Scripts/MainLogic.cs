using Controllers;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

namespace DavideCTest
{
    public class MainLogic : MonoBehaviour
    {
        public static MainLogic instance;

        [SerializeField] private GameObject playerModel;
        [SerializeField] private Vector3 spawnLocation = Vector3.zero;
        [SerializeField] private Transform playerContainer;

        private float debugMaxTime = 1f;
        private float debugTimer = 0f;

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

        private void Update()
        {
            debugTimer -= Time.deltaTime;
            if(debugTimer < 0)
                debugTimer = 0;

            if(Input.GetKeyDown(KeyCode.H) && debugTimer <= 0f)
            {
                debugTimer = 1f;

                PlayersHpToConsole();
            }
        }

        public void SpawnPlayer()
        {
            Instantiate(playerModel, spawnLocation, Quaternion.Euler(0, 0, 0), playerContainer);
        }

        public void PlayersHpToConsole()
        {
            if (playerContainer.GetComponentInChildren<TempPlayerController>() != null)
            {
                foreach (var player in playerContainer.GetComponentsInChildren<TempPlayerController>())
                {
                    Debug.Log("Current HP " + player.name + " = " + player.CurrentHp);
                }
            }
        }
    }
}
