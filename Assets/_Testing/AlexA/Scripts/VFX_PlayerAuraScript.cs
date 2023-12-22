using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_PlayerAuraScript : MonoBehaviour
{
    [SerializeField] private Material lv0Material;
    [SerializeField] private Material lv1Material;
    [SerializeField] private Material lv2Material;
    [SerializeField] private Material lv3Material;
    
    private GameObject vfx_PlayerAuraCapsule;
    private GameObject vfx_PlayerAuraBase;

    private float maxSize;
    private float minSize;
    private float currentSize;

    private float CurrentSize 
    {
        get { return currentSize; }
        set 
        {
            if (value < minSize)
            {
                currentSize = minSize;
            }
            else if (value > maxSize)
            {
                currentSize = maxSize;
            }
            else
                currentSize = value;
        }
    }

    private void OnChargingStarted() 
    {
        vfx_PlayerAuraBase.SetActive(true);
        vfx_PlayerAuraCapsule.SetActive(true);
    }
    
    private void OnChargingUpdate() 
    {
    }
}
