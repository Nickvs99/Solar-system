using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : CelestialBody
{

    public enum StarType {
        LocalStar,
        DistantStar
    }

    public void Initialize(StarType starType)
    {
        Mass = Distribution.GenerateSolarMass();

        Density = 0.1f;

        Radius = CalcRadius(Mass, Density);

        gameObject.layer = LayerMask.NameToLayer("Star");

        if(starType == StarType.DistantStar){
            Destroy(this.GetComponent<Light>());
        }
    }
}