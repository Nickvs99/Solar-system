using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : CelestialBody 
{    
    public void Initialize()
    {    
        mass = Distribution.GeneratePlanetMass();

        density = 0.01f;

        radius = CalcRadius(mass, density);
    }
}