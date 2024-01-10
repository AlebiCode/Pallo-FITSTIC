using Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PalloTrigger : MonoBehaviour
{

    [SerializeField] private UnityEvent<PalloController> onPalloEnter = new UnityEvent<PalloController>();

    public UnityEvent<PalloController> OnPalloEnter => onPalloEnter;

}
