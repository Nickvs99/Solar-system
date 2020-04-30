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
        if (playing)
        {
            UpdateSystem();
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

    public void RespawnSystem()
    {
        SetSeed();
        solarSystem.ClearBodies();
        solarSystem.SpawnBodies(bodies, initial_size);
    }

    public void FlipPlayState()
    {
        playing = !playing;
    }

    public void ManualUpdate()
    {
        playing = false;
        UpdateSystem();
    }

    void UpdateSystem()
    {
        solarSystem.UpdateBodies();
        solarSystem.CheckCollisions();
        Camera.main.GetComponent<CameraHandler>().UpdatePosition();
    }
}
