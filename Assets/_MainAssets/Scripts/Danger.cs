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
using StateMachine;

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

    [SerializeField] private float rejectionForce = 30f;
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private bool isActive = false;

    //[SerializeField] public float atPositionForceHorizontal = 2f;
    //[SerializeField] public float atPositionForceVertical = 5f;

    [SerializeField] private DangersConfig dangerConfig;
    public DangersConfig DangerConfig => dangerConfig;

    private void Awake()
    {
        if (DangerConfig.isMovable)
        {
            // Attiva la possibilità di spostare il Danger tramite la fisica
            Rigidbody rb = this.GetComponent<Rigidbody>();
            rb.isKinematic = false;
        }
    }

    // Quando la palla colpisce questo oggetto si attiva effetto diverso a seconda del tipo di pericolo (distinzione in base al colore)

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            DangerBehavior();
        }
        if (collision.gameObject.layer == 7)
        {
            //aggiungi logica palla contro trappola
            //DangerBehaviorForBall();
        }
    }

    private void DangerBehavior()
    {
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
                            // Comportamento del RedDanger
                            // Porta a zero la vita del personaggio vicino

                            player.gameObject.GetComponentInChildren<TestPlayerController>().KillPlayer();

                            //prima di distruzione esegui VFX and SFX

                            Destroy(this.gameObject);

                            break;

                        case DangerTypesEnum.Purple:
                            // Comportamento del PurpleDanger
                            // Diminuisce la stamina di valore X
                            // Respinge il personaggio vicino

                            DangerHitEffect();

                            StaminaDecrese(player);

                            RejectPlayer(player);

                            Debug.Log("damage taken = " + dangerConfig.explosionDamage);

                            break;

                        case DangerTypesEnum.Yellow:
                            // Comportamento del YellowDanger
                            // Diminuisce la stamina di valore X del personaggio vicino

                            DangerHitEffect();

                            StaminaDecrese(player);

                            Debug.Log("damage taken = " + dangerConfig.explosionDamage);

                            break;
                    }
                }
            }
        }
    }


    public Coroutine scaleTweenCoroutine;
    public Tween scalingTween;
    public Vector3 hitScale = Vector3.one;
    public Vector3 hitScaleFinal = new Vector3(1.1f, 1.1f, 1.1f);
    private void DangerHitEffect()
    {
            if (scaleTweenCoroutine == null)
            {
                scaleTweenCoroutine = StartCoroutine(scalingRoutine());
            }

            IEnumerator scalingRoutine()
            {
                if (this.gameObject != null)
                {
                    var danger = this.gameObject;
                    //scalingTween = player.transform.DOScale(hitScaleFinal, .2f);
                    if (scalingTween != null && scalingTween.active)
                    {
                        scalingTween.Kill(false);
                        scalingTween = null;
                    danger.transform.localScale = Vector3.one;
                    }

                    scalingTween = danger.transform.DOPunchScale(hitScaleFinal, .2f, 6, 0.1f);
                    yield return new WaitForSeconds(.2f);

                }
                else
                {
                    yield return null;
                }
            }

            scaleTweenCoroutine = null;
    }

    private void RejectPlayer(Collider _player)
    {
        if(!_player.GetComponent<TestPlayerController>().playerIsBeingRejected)
            _player.GetComponent<TestPlayerController>().SetPlayerToRejectState();

        //player.attachedRigidbody.AddExplosionForce(rejectionForce, this.transform.position, 5f);

        Vector3 forcedirection = (new Vector3(_player.transform.position.x, _player.transform.position.y + 1f, _player.transform.position.z) - this.transform.position).normalized;
        _player.attachedRigidbody.AddForce(forcedirection * rejectionForce, ForceMode.Impulse);

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
