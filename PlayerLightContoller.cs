using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class PlayerLightContoller : MonoBehaviour
{
    public float flickeringAmount;
    public float innerRadiusAmount;
    Light2D myLight;
    private float intensity_original;
    private float innerRadius_original;

    void Start()
    {
        myLight = GetComponent<Light2D>();
        intensity_original = myLight.intensity;
        innerRadius_original = myLight.pointLightInnerRadius;
    }

    void Update()
    {
        myLight.intensity = Random.Range(intensity_original - flickeringAmount, intensity_original + flickeringAmount);
        myLight.pointLightInnerRadius = Random.Range(innerRadius_original - innerRadiusAmount, innerRadius_original + innerRadiusAmount);
    }
}
