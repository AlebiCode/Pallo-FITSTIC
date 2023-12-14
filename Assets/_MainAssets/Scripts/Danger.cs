using Controllers;
using DavideCTest;
using System.Collections;
using System.Collections.Generic;
using LorenzoCastelli;
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
    [SerializeField] private int purpleDamage = 20;


    [SerializeField] private float explosionRadius = 2f;

    // Quando la palla colpisce questo oggetto si attiva effetto diverso a seconda del tipo di pericolo (distinzione in base al colore)

  
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetType() == typeof(CapsuleCollider))
        {
            if (collision.gameObject.layer == 6)
            {
                DangerBehavior();
            }
            if (collision.gameObject.layer == 7)
            {
                //aggiungi logica palla contro trappola
            }
        }
    }

    private void DangerBehavior()
    {
        if (isMovable)
        {
            // Attiva la possibilità di spostare il Danger tramite la fisica
            Rigidbody rb = this.GetComponent<Rigidbody>();

            rb.isKinematic = false;
        }
        if (isActive)
        {
            var playersHit = Physics.OverlapSphere(this.transform.position, 2f, 1 << 6);

            if (playersHit.Length > 0)
            {

                if (isRed)
                {
                    foreach (var player in playersHit )
                    {
                        KillPlayer(player.gameObject.GetComponentInChildren<TempPlayerController>());

                    }

                    //prima di distruzione esegui VFX and SFX

                    Destroy(this.gameObject);

                    // Comportamento del RedDanger
                    // Porta a zero la vita del personaggio vicino
                }
                if (isPurple)
                {
                    foreach (var player in playersHit)
                    {
                        player.gameObject.GetComponentInChildren<PlayerData>().TakeDamage(purpleDamage);
                        Debug.Log("damage taken = " + purpleDamage);
                    }

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
    }

    private void KillPlayer(TempPlayerController player)
    {
        Debug.Log("player is killed!");
        //rimuovi destroy e togli una vita
        Destroy(player.gameObject);
    }

    private void StaminaDecrese()
    {

    }

    private void RejectPlayer()
    {

    }

}
