using System.Collections;
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
    private bool playing = true;

    void Start()
    {

        SetSeed();

        solarSystem.SpawnBodies(bodies, initial_size);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RespawnSystem();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)){
            playing = !playing;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            playing = false;
            solarSystem.UpdateBodies();
            solarSystem.CheckCollisions();
        }

        if (playing)
        {
            solarSystem.UpdateBodies();
            solarSystem.CheckCollisions();
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
}
