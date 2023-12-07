using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danger : MonoBehaviour
{
    // I pericoli possono diminuire la stamina, applicare una forza respingente e portare alla perdita di una vita
    [Header("Type of Danger")]
    [SerializeField] private bool isRed;
    [SerializeField] private bool isPurple;
    [SerializeField] private bool isYellow;
    [SerializeField] private bool isMovable;
    [SerializeField] private bool isActive;

    // Quando la palla colpisce questo oggetto si attiva effetto diverso a seconda del tipo di pericolo (distinzione in base al colore)
    private void DangerBehavior()
    {
        if (isMovable)
        {
            // Attiva la possibilità di spostare il Danger tramite la fisica
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }
        if (isActive)
        {
            if (isRed)
            {
                // Comportamento del RedDanger
                // Porta a zero la vita del personaggio vicino
            }
            if (isPurple)
            {
                // Comportamento del PurpleDanger
                // Diminuisce la stamina di valore X
                // Respinge il personaggio vicino
            }
            if (isYellow)
            {
                // Comportamento del YellowDanger
                // Diminuisce la stamina di valore X del personaggio vicino
            }
        }
    }

    private void Explosion()
    {

    }

    private void StaminaDecrese()
    {

    }

    private void RejectPlayer()
    {

    }

}
