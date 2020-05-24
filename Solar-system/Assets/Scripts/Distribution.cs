using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Distribution {
    
    private static bool avg = false;
    private static float starCutOff = 5 * Mathf.Pow(10, 8);
    
    public static float GenerateNormalValue(float mu, float sigma)
    {
        float r1 = Random.Range(0.0f, 1.0f);
        float r2 = Random.Range(0.0f, 1.0f);

        float n = Mathf.Sqrt(-2f * Mathf.Log(r1)) * Mathf.Cos((2f * Mathf.PI) * r2);

        return (mu + sigma * n);
    }

    public static float GenerateLogNormalValue(float mu, float sigma)
    {
        return Mathf.Pow(10, GenerateNormalValue(mu, sigma));
    }
    
    public static float GenerateSolarMass()
    {   
        float avgSolarMass = Mathf.Pow(10, 10);
        
        if (avg) {return avgSolarMass;}

        float solarMass;
        do {
            solarMass = GenerateLogNormalValue(0f, 0.7f) * avgSolarMass;
        }
        while (solarMass < starCutOff);
        
        return solarMass;
    }

    public static float GenerateDistBinarySystem(float totalStarRadii)
    {
        float avgDistBinarySystem = 20000f;

        if (avg) {return avgDistBinarySystem;}
        
        // Makes sure the stars are at least further apart than there combine radii
        float dist;
        do {
            dist = GenerateLogNormalValue(0f, 0.7f) * avgDistBinarySystem;
        }
        while (dist < totalStarRadii * 1.1f);

        return dist;
    }

    public static float GenerateSemiMajorAddition()
    {
        float avgAddition = 40000f;
        
        if (avg) {return avgAddition;}

        return GenerateLogNormalValue(0f, 0.3f) * avgAddition;
    }

    public static float GeneratePlanetMass()
    {
        float avgPlanetMass = Mathf.Pow(10, 7);

        if (avg) {return avgPlanetMass;}

        float planetMass;
        do {
            planetMass = GenerateLogNormalValue(0f, 0.7f) * avgPlanetMass;
        }
        while (planetMass > starCutOff);

        return planetMass;
    }
}