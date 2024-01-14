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

    private void OnDisable()
    {
        myPalloScript.OnStateChange.RemoveListener(OnChangeBallState);
    }

    private void Start()
    {
        Init();    
    }

    private void Init() 
    {
        vfx_BallAuraCapsule = GetComponentInChildren<GameObject>();
        vfx_BallAuraTrail = vfx_BallAuraCapsule.GetComponentInChildren<GameObject>();
        myPallo = transform.parent.gameObject;
        myPalloScript = myPallo.GetComponent<PalloController>();
        myPalloScript.OnStateChange.AddListener(OnChangeBallState);
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
        if(state == PalloController.BallStates.thrown) 
        {
            Vector3 direction;
            CheckLevel(myPalloScript.SpeedTier);
            direction = gameObject.transform.InverseTransformDirection(myPallo.GetComponent<Rigidbody>().velocity);
            transform.forward = direction;
            vfx_BallAuraCapsule.SetActive(true);
        }
        else 
        {
            vfx_BallAuraCapsule.SetActive(false);
        }
    }
}
