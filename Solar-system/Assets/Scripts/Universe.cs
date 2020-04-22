using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universe : MonoBehaviour
{
    public SolarSystem solarSystem;

    [Header("Seed generator")]
    public bool randomSeed;
    public int seed;

    void Start()
    {

        SetSeed();

        solarSystem.SpawnBodies(3, 10f);
    }

    void Update()
    {
        solarSystem.UpdateBodies();
    }

    void SetSeed()
    {
        if (randomSeed)
        {
            seed = (int) Random.Range(0, 2147483647);
            Random.InitState(seed);
        }
        else
        {
            Random.InitState(seed);
        }

        Debug.Log("SEED: " + seed);
    }
}
