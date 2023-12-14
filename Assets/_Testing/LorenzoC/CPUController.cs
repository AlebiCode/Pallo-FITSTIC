using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using LorenzoCastelli;
using UnityEngine;
using UnityEngine.AI;

namespace LorenzoCastelli { 

public class CPUController : PlayerControlsGeneric {

    [SerializeField] private float maxChargeTime = 1;
    [SerializeField] private float maxThrowForce = 10;

    [SerializeField] public PlayerData playerData;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 1;
    [SerializeField] private Transform handsocket;


    private NavMeshAgent ai = new NavMeshAgent();

    public GameObject frontPoint;

    private PalloController heldPallo;

    public GameObject currentTarget;
    public float lookRadius = 6.66f;
    public float turningSpeed = 10f;

    public override void BallThrow() {
        throw new System.NotImplementedException();
    }

    public override void PlayerMovement() {
        if (heldPallo) {
            //SCEGLI UN BERSAGLIO OPPURE MUOVITI DA QUALCHE PARTE
            
        } else {
            //CONTROLLO SE LA PALLA L'HA PRESO QUALCUNO OPPURE NO
        ai.SetDestination(GameObject.FindGameObjectWithTag("Pallo").transform.position);
        }
    }

    public override void PlayerRotation() {
        if (!currentTarget) {
            //SE NON STO GUARDANDO NESSUNO CERCA UN NUOVO TARGET
            Vector3 fwd = frontPoint.transform.TransformDirection(Vector3.forward);
            RaycastHit hit;
            if (Physics.Raycast(frontPoint.transform.position, fwd, out hit, 50)){
                Debug.DrawRay(frontPoint.transform.position, fwd, Color.red, 2);
                
                if (hit.collider.gameObject.GetComponent<PalloController>()) {
                    //OBIETTIVO è PALLO
                    Debug.Log("Current look target for " + gameObject.name + " is " + currentTarget);
                } else if (hit.collider.gameObject.GetComponent<PlayerControlsGeneric>()) {
                    //OBIETTIVO è UN PLAYER
                    Debug.Log("Current look target for " + gameObject.name + " is " + currentTarget);
                }
                    currentTarget = hit.collider.gameObject;
                    LookTarget(currentTarget);

            }
        } else {
            if ((currentTarget.transform.position.x- transform.position.x > 50) || (currentTarget.transform.position.x - transform.position.x < -50) || (currentTarget.transform.position.z - transform.position.z > 50) || (currentTarget.transform.position.z - transform.position.z < -50)) {
                currentTarget = null;
            } else {
                LookTarget(currentTarget);
            }
        }
        
    }

    private void LookTarget(GameObject target) {
        if (target) {
            Quaternion _lookRotation = Quaternion.LookRotation((target.transform.position - transform.position).normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * turningSpeed);
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        ai = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
            PlayerRotation();

    }
}
}