using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TrapManagerTest : MonoBehaviour
{
    [SerializeField] private List<GameObject> traps;
    [SerializeField] private List<Transform> spawnPoints;

    private void Start()
    {
        AssignObjectsToRandomPositions();
        StartCoroutine(CheckAndReactivateObjects());
    }

    private void AssignObjectsToRandomPositions()
    {
        // Verifica se ci sono oggetti e posizioni disponibili
        if (traps.Count == 0 || spawnPoints.Count == 0)
        {
            Debug.LogError("Assicurati di avere almeno un oggetto e una posizione disponibile.");
            return;
        }

        // Assicurati che ci siano abbastanza posizioni disponibili per gli oggetti
        if (traps.Count > spawnPoints.Count)
        {
            Debug.LogError("Non ci sono abbastanza posizioni disponibili per tutti gli oggetti.");
            return;
        }

        foreach (GameObject trap in traps)
        {
            // Disattiva l'oggetto corrente
            trap.SetActive(false);

            // Scegli una posizione casuale e assegna l'oggetto
            int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
            Transform randomPosition = spawnPoints[randomIndex];

            // Attiva l'oggetto nella posizione casuale
            trap.transform.position = randomPosition.position;
            trap.transform.rotation = randomPosition.rotation;
            trap.SetActive(true);

            // Rimuovi la posizione assegnata dalla lista delle posizioni disponibili
            spawnPoints.RemoveAt(randomIndex);

            Debug.Log(trap.name + " assegnato a posizione casuale!");
        }

    }

 
    private IEnumerator CheckAndReactivateObjects()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);

            // Verifica e riattiva gli oggetti disattivati
            ReactivateDeactivatedObjects();
        }
    }

    private void ReactivateDeactivatedObjects()
    {
        foreach (GameObject trap in traps)
        {
            if (trap != null && !trap.activeSelf)
            {
                // Riattiva l'oggetto disattivato
                trap.SetActive(true);
                Debug.Log(trap.name + " riattivato!");
            }
        }
    }
}

