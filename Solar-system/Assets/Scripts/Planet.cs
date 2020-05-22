using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : CelestialBody 
{    
    public void Initialize(Vector3 _position, Vector3 _velocity, float _mass)
    {    
        density = 0.01f;

        SetValues(_position, _velocity, _mass, density);
    }
}