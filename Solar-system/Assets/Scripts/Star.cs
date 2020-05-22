using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : CelestialBody
{
    public void Initialize(Vector3 _position, Vector3 _velocity, float _mass)
    {
        density = 0.1f;

        SetValues(_position, _velocity, _mass, density);

        this.GetComponent<Renderer>().material.color = Color.yellow;
    }
}