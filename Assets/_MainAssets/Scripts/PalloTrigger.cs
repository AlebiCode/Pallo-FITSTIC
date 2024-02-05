using Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PalloTrigger : MonoBehaviour
{
    //[SerializeField] private bool isActive = true;
    [SerializeField] protected UnityEvent<PalloController> onPalloEnter = new UnityEvent<PalloController>();
    [SerializeField] protected UnityEvent<PalloController> onPalloExit = new UnityEvent<PalloController>();

    private void Start()
    {
        //mi serve questo start vuoto per far apparire l'icona per attivare/disattivare il component nell'inspector. Che bello!
    }   

    public void SetActivationStatus(bool value)
    {
        enabled = value;
    }

    public void CallPalloTriggerEnter(PalloController palloController)
    {
        if (enabled)
            onPalloEnter.Invoke(palloController);
    }
    public void CallPalloTriggerExit(PalloController palloController)
    {
        if (enabled)
            onPalloExit.Invoke(palloController);
    }

    public void AddOnEnterListener(UnityAction<PalloController> unityAction)
    {
        onPalloEnter.AddListener(unityAction);
    }
    public void AddOnExitListener(UnityAction<PalloController> unityAction)
    {
        onPalloExit.AddListener(unityAction);
    }





}
