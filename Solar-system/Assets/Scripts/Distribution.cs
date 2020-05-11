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

    public static float GenerateSolarMass()
    {   
        float r = GenerateNormalValue(0f, 0.7f);
        return Mathf.Pow(10, r) * Constants.SolarMass;
    }
}