using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;

public class VFX_BallAuraScript : MonoBehaviour
{
    [SerializeField] private List<Material> capsuleMaterials;
    [SerializeField] private List<Material> trailMaterials;

    private GameObject vfx_BallAuraCapsule;
    private GameObject vfx_BallAuraTrail;
    private GameObject myPallo;

    private PalloController myPalloScript;

    private Vector3 currentPosition;
    private Vector3 previousPosition;

    private bool isMovingAndActive;
    private void OnEnable()
    {
        GetAndListenToMyPallo();
    }

    private void OnDisable()
    {
        StopListeningToMyPallo();
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        HandleRotation();
    }

    private void Init() 
    {
        vfx_BallAuraCapsule = transform.GetChild(0).gameObject;
        vfx_BallAuraTrail = vfx_BallAuraCapsule.transform.GetChild(0).gameObject;

        currentPosition = transform.position;  
    }

    private void CheckLevel(int level) 
    {
        switch (level) 
        {
            case 0:
                vfx_BallAuraCapsule.GetComponent<ParticleSystemRenderer>().material = capsuleMaterials[0];
                vfx_BallAuraTrail.GetComponent<TrailRenderer>().material = trailMaterials[0];
                break;

            case 1:
                vfx_BallAuraCapsule.GetComponent<ParticleSystemRenderer>().material = capsuleMaterials[1];
                vfx_BallAuraTrail.GetComponent<TrailRenderer>().material = trailMaterials[1];
                break;

            case 2:
                vfx_BallAuraCapsule.GetComponent<ParticleSystemRenderer>().material = capsuleMaterials[2];
                vfx_BallAuraTrail.GetComponent<TrailRenderer>().material = trailMaterials[2];
                break;

            case 3:
                vfx_BallAuraCapsule.GetComponent<ParticleSystemRenderer>().material = capsuleMaterials[3];
                vfx_BallAuraTrail.GetComponent<TrailRenderer>().material = trailMaterials[3];
                break;
        }
    }

    private void OnChangeBallState(PalloController.BallStates state) 
    {
        Debug.Log(state);
        if(state == PalloController.BallStates.thrown) 
        {
            CheckLevel(myPalloScript.SpeedTier);
            vfx_BallAuraCapsule.SetActive(true);
            isMovingAndActive = true;
        }
        else 
        {
            vfx_BallAuraCapsule.SetActive(false);
            isMovingAndActive = false;
        }
    }

    private void HandleRotation() 
    {
        if (isMovingAndActive) 
        { 
            previousPosition = currentPosition;
            currentPosition = transform.position;

            if (currentPosition - previousPosition != Vector3.zero)
            {
                transform.forward = previousPosition - currentPosition;
            }
        }
    }

    private void GetAndListenToMyPallo() 
    {
        if (!myPalloScript)
        {
            myPallo = transform.parent.gameObject;
            myPalloScript = myPallo.GetComponent<PalloController>();
            myPalloScript.OnStateChange.AddListener(OnChangeBallState);

        }
        else
            myPalloScript.OnStateChange.AddListener(OnChangeBallState);
    }

    private void StopListeningToMyPallo() 
    {
        myPalloScript.OnStateChange.RemoveListener(OnChangeBallState);
    }
}
