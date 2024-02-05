using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX_Flame : MonoBehaviour
{
    [SerializeField] private float intensityChange;
    [SerializeField] private float rateDamping;
    [SerializeField] private float strenght;
    [SerializeField] private float baseIntensity;
    private Light flickeringLight;
    private void Start()
    {
        flickeringLight = GetComponent<Light>();
        baseIntensity = flickeringLight.intensity;
    }
    private void Update()
    {
        StartCoroutine(DoFlicker());
    }
    private IEnumerator DoFlicker() 
    {
        flickeringLight.intensity = Mathf.Lerp(flickeringLight.intensity, Random.Range(baseIntensity - intensityChange, baseIntensity + intensityChange), strenght * Time.deltaTime);
        yield return new WaitForSeconds(rateDamping);
    }

}
