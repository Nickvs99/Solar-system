using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Distribution {
    
    private static bool avg = false;
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

        return GenerateLogNormalValue(0f, 0.7f) * avgSolarMass;
    }

    public static float GenerateDistBinarySystem()
    {
        float avgDistBinarySystem = 20000f;

        if (avg) {return avgDistBinarySystem;}

        return GenerateLogNormalValue(0f, 0.4f) * avgDistBinarySystem;
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

        return GenerateLogNormalValue(0f, 0.9f) * avgPlanetMass;
    }
}