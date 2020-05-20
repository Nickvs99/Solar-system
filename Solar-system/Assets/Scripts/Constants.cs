using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public static float G = 0.1f;   // Universal gravity constant
    public static float SolarMass = 100f; // Mean solar mass
    public static float PlanetMass = SolarMass / 1000f;  // Mean Planetary mass
    public static float BinaryDistance = 100f; // Mean distance between stars in a binary system
    public static float PlanetDistance = 100f; // Mean distance between planets

    public static float density = 0.3f; // Temp, since density does not change per body
}
