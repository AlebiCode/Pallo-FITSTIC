using Controllers;
using DavideCTest;
using System.Collections;
using System.Collections.Generic;
using LorenzoCastelli;
using UnityEngine;
using Unity.VisualScripting;
using System;
using TMPro;
using DG.Tweening;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Danger : MonoBehaviour
{
    // I pericoli possono diminuire la stamina, applicare una forza respingente e portare alla perdita di una vita
    //[Header("Type of Danger")]

    public enum DangerTypesEnum
    {
        None = 0,
        Red = 10,
        Purple = 11,
        Yellow = 12,
    }

    [SerializeField] private float explosionForce = 20f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private bool isActive = false;
    [SerializeField] private bool isPlayerBeingRejected = false;

    //[SerializeField] public float atPositionForceHorizontal = 2f;
    //[SerializeField] public float atPositionForceVertical = 5f;

    [SerializeField] private DangersConfig dangerConfig;

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
        if (dangerConfig.isMovable)
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
                    switch(dangerConfig.DangerType)
                    {
                        case DangerTypesEnum.Red:

                            player.gameObject.GetComponentInChildren<TestPlayerController>().KillPlayer();

                            //prima di distruzione esegui VFX and SFX

                            Destroy(this.gameObject);

                            // Comportamento del RedDanger
                            // Porta a zero la vita del personaggio vicino
                            break;

                        case DangerTypesEnum.Purple:


                            StaminaDecrese(player);

                            RejectPlayer(player);

                            Debug.Log("damage taken = " + dangerConfig.explosionDamage);

                            // Comportamento del PurpleDanger
                            // Diminuisce la stamina di valore X
                            // Respinge il personaggio vicino
                            break;

                        case DangerTypesEnum.Yellow:


                            StaminaDecrese(player);

                            Debug.Log("damage taken = " + dangerConfig.explosionDamage);

                            // Comportamento del YellowDanger
                            // Diminuisce la stamina di valore X del personaggio vicino
                            break;
                    }
                }
            }
        }
    }

    private void RejectPlayer(Collider _player)
    {
        if(!_player.GetComponent<TestPlayerController>().playerIsBeingRejected)
            _player.GetComponent<TestPlayerController>().SetPlayerToRejectState();

        //player.attachedRigidbody.AddExplosionForce(explosionForce, this.transform.position, 5f);

        Vector3 forcedirection = (new Vector3(_player.transform.position.x, _player.transform.position.y + 1f, _player.transform.position.z) - this.transform.position).normalized;
        _player.attachedRigidbody.AddForce(forcedirection * explosionForce, ForceMode.Force);

        //Vector3 finalForce = new Vector3(forcedirection.x * atPositionForceHorizontal, forcedirection.y * atPositionForceVertical, forcedirection.z * atPositionForceHorizontal);
    }

    private void StaminaDecrese(Collider _player)
    {
        _player.gameObject.GetComponentInChildren<TestPlayerController>().HitScaling();

        _player.gameObject.GetComponentInChildren<TestPlayerController>().TakeDamage(dangerConfig.explosionDamage);

    }

   

    private void FixedUpdate()
    {
    }

}
