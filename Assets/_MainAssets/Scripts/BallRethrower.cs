using Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRethrower : PalloTrigger
{
    [SerializeField] private float stasisTime = 2.0f;
    [SerializeField] private Transform targetDirectionPoint;
    [SerializeField] private float verticalForce = 5;
    [SerializeField] private float horizontalForce = 5;

    void Start()
    {
        onPalloEnter.AddListener(RethrowPallo);
    }

    private void RethrowPallo(PalloController palloController)
    {
        StartCoroutine(RethrowPallo_Co(palloController));
    }

    private IEnumerator RethrowPallo_Co(PalloController palloController)
    {
        palloController.BallState = PalloController.BallStates.held;
        yield return new WaitForSeconds(stasisTime);
        Vector3 force = (targetDirectionPoint.position - palloController.transform.position);
        force.y = 0;
        force = force.normalized * horizontalForce;
        force.y = verticalForce;
        palloController.Respawn(force);
    }

    /*
    private bool SonoInRangeDiPickup()
    {
        float pickupAngle = 180;    //in gradi, quanto è ampia la zona in cui grabbi la palla
        float angle = Vector3.Angle(Vector3.forward, player.InverseTransformPoint(pallo.transform.position));
        return angle <= pickupAngle / 2;
    }
    */
}
