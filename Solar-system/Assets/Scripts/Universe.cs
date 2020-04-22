﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universe : MonoBehaviour
{
    public SolarSystem solarSystem;

    [Header("Solar system properties")]
    public int bodies;
    public float initial_size;

    [Header("Seed generator")]
    public bool randomSeed;
    public int seed;

    void Start()
    {

        SetSeed();

        solarSystem.SpawnBodies(bodies, initial_size);
    }

    void Update()
    {
        solarSystem.UpdateBodies();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RespawnSystem();
        }
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

    void RespawnSystem()
    {
        SetSeed();
        solarSystem.ClearBodies();
        solarSystem.SpawnBodies(bodies, initial_size);
    }

    private void OnDrawGizmos()
    {
        Vector3 com = solarSystem.CalcCenterOfMass();

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(com, 1f);
    }
}
