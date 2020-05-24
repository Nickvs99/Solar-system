using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : CelestialBody
{
    public void Initialize()
    {
        mass = Distribution.GenerateSolarMass();

        density = 0.1f;

        radius = CalcRadius(mass, density);
        
        this.GetComponent<Renderer>().material.color = Color.yellow;
    }
}