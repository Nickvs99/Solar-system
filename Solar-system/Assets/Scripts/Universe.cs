using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universe : MonoBehaviour
{
    public SolarSystem solarSystem;

    void Start()
    {
        solarSystem.SpawnBodies();
    }

    void Update()
    {
        solarSystem.UpdateBodies();
    }
}
