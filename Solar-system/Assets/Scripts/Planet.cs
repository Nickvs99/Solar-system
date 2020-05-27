using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : CelestialBody 
{    
    public void Initialize()
    {    
        Mass = Distribution.GeneratePlanetMass();

        Density = 0.01f;

        Radius = CalcRadius(Mass, Density);
    }
}