using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Distribution {
    
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
        return GenerateLogNormalValue(0f, 0.7f) * 100f;
    }

    public static float GenerateDistBinarySystem()
    {
        return GenerateLogNormalValue(0f, 0.4f) * 100f;
    }

    public static float GenerateSemiMajorAddition()
    {
        return GenerateLogNormalValue(0f, 0.5f) * 100f;
    }

    public static float GeneratePlanetMass()
    {
        return GenerateLogNormalValue(0f, 0.9f) * 0.1f;
    }
}