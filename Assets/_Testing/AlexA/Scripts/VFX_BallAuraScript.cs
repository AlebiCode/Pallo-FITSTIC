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

    private PalloController myPallo;

    private void OnDisable()
    {
        myPallo.OnStateChange.RemoveListener(OnChangeBallState);
    }

    private void Start()
    {
        Init();    
    }

    private void Init() 
    {
        vfx_BallAuraCapsule = GetComponentInChildren<GameObject>();
        vfx_BallAuraTrail = vfx_BallAuraCapsule.GetComponentInChildren<GameObject>();
        myPallo = transform.parent.GetComponent<PalloController>();
        myPallo.OnStateChange.AddListener(OnChangeBallState);
    }

    private void CheckLevel(int level) 
    {
        switch (level) 
        {
            case 0:
                vfx_BallAuraCapsule.GetComponent<ParticleSystemRenderer>().material = capsuleMaterials[0];
                vfx_BallAuraTrail.GetComponent<ParticleSystemRenderer>().material = trailMaterials[0];
                break;

            case 1:
                vfx_BallAuraCapsule.GetComponent<ParticleSystemRenderer>().material = capsuleMaterials[1];
                vfx_BallAuraTrail.GetComponent<ParticleSystemRenderer>().material = trailMaterials[1];
                break;

            case 2:
                vfx_BallAuraCapsule.GetComponent<ParticleSystemRenderer>().material = capsuleMaterials[2];
                vfx_BallAuraTrail.GetComponent<ParticleSystemRenderer>().material = trailMaterials[2];
                break;

            case 3:
                vfx_BallAuraCapsule.GetComponent<ParticleSystemRenderer>().material = capsuleMaterials[3];
                vfx_BallAuraTrail.GetComponent<ParticleSystemRenderer>().material = trailMaterials[3];
                break;
        }
    }

    private void OnChangeBallState(PalloController.BallStates state) 
    { 
        if(state != PalloController.BallStates.thrown) 
        {
            vfx_BallAuraCapsule.SetActive(false);
            vfx_BallAuraTrail.SetActive(false);
        }
        else 
        {
            CheckLevel(myPallo.SpeedTier);
            vfx_BallAuraCapsule.SetActive(true);
            vfx_BallAuraTrail.SetActive(true);
        }
    }
}
