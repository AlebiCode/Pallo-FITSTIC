using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;
using UnityEngine.AI;

public class CPUController : MonoBehaviour
{
    private CharacterController ccontroller = new CharacterController();
    private TempPlayerController TPC = new TempPlayerController();
    private NavMeshAgent ai = new NavMeshAgent(); 
    // Start is called before the first frame update
    void Awake()
    {
        TPC=gameObject.GetComponent<TempPlayerController>();
        ccontroller = gameObject.GetComponent<CharacterController>();
        ai = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        ai.SetDestination(GameObject.FindGameObjectWithTag("Pallo").transform.position);
    }
}
