using Controllers;
using DavideCTest;
using System.Collections;
using System.Collections.Generic;
using LorenzoCastelli;
using UnityEngine;
using Unity.VisualScripting;

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
    [SerializeField] private int yellowDamage = 20;
    [SerializeField] private float atPositionForceHorizontal = 2f;
    [SerializeField] private float atPositionForceVertical = 5f;
    [SerializeField] private float explosionForce = 20f;
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
            var playersHit = Physics.OverlapSphere(this.transform.position, explosionRadius, 1 << 6 );

            if (playersHit.Length > 0)
            {
                foreach (var player in playersHit)
                {
                    if (isRed)
                    {
                        player.gameObject.GetComponentInChildren<TempPlayerController>().KillPlayer();

                       //prima di distruzione esegui VFX and SFX

                        Destroy(this.gameObject);

                        // Comportamento del RedDanger
                        // Porta a zero la vita del personaggio vicino
                    }
                    if (isPurple)
                    {
                        player.gameObject.GetComponentInChildren<PlayerData>().TakeDamage(purpleDamage);

                        RejectPlayer(player);

                        Debug.Log("damage taken = " + purpleDamage);

                        // Comportamento del PurpleDanger
                        // Diminuisce la stamina di valore X
                        // Respinge il personaggio vicino
                    }
                    if (isYellow)
                    {
                        player.gameObject.GetComponentInChildren<PlayerData>().TakeDamage(yellowDamage);

                        // Comportamento del YellowDanger
                        // Diminuisce la stamina di valore X del personaggio vicino
                    }
                }
            }
        }
    }

    private void RejectPlayer(Collider _player)
    {
        //player.attachedRigidbody.AddExplosionForce(explosionForce, this.transform.position, 5f);

        Vector3 forcedirection = (new Vector3(_player.transform.position.x, _player.transform.position.y + 1f, _player.transform.position.z) - this.transform.position).normalized;
        _player.attachedRigidbody.AddForce(forcedirection * explosionForce, ForceMode.Impulse);

        //Vector3 finalForce = new Vector3(forcedirection.x * atPositionForceHorizontal, forcedirection.y * atPositionForceVertical, forcedirection.z * atPositionForceHorizontal);
    }

    private void StaminaDecrese()
    {

    }

    

}
