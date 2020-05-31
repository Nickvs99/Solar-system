using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : CelestialBody
{
    public void Initialize()
    {
        Mass = Distribution.GenerateSolarMass();

        Density = 0.1f;

        Radius = CalcRadius(Mass, Density);

        gameObject.layer = LayerMask.NameToLayer("Star");
    }
}