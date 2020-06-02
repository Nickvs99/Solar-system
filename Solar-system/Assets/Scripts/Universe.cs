using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Universe : MonoBehaviour
{
    public Star starPrefab;

    public Planet planetPrefab;
    
    public SolarSystem solarSystem;

    public enum SpawnMethods
    {
        SpawnBlock, 
        SpawnOrbital
    }

    public SpawnMethods spawnmethod;

    [Header("Spawn block properties")]
    public int bodies;
    public float initialSize;

    [Header("Spawn orbital properties")]
    [SerializeField]
    private int DistantStarCount = 100;

    [Header("Seed generator")]
    public bool randomSeed;
    public int seed;

    [Header("Physics engine")]
    public float timeStep = 1f;                 // Speed of simulation
    public int collisionCheckPerTimeStep = 1;   // Increase value to get a better precision

    private bool playing = true;

    void Start()
    {
        SpawnSystem();
        SpawnDistantStars();
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
            seed = (int) Random.Range(-2147483647, 2147483647);
            Random.InitState(seed);
        }
        else
        {
            Random.InitState(seed);
        }

        Debug.Log("SEED: " + seed);
    }

    public void SpawnSystem()
    {
        SetSeed();
        solarSystem.ClearBodies();

        switch (spawnmethod)
        {
            case SpawnMethods.SpawnBlock:
                solarSystem.SpawnBodiesBlock(bodies, initialSize);
                break;
            case SpawnMethods.SpawnOrbital:
                Camera.main.GetComponent<CameraHandler>().SetOrigin();
                solarSystem.SpawnBodiesOrbital();
                break;
        }

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
        int collisionsCheck = (int) Mathf.Ceil(timeStep * collisionCheckPerTimeStep);

        // makes sure that at low timesteps there are still multiple collisions checks
        collisionsCheck = Mathf.Max(collisionsCheck, collisionCheckPerTimeStep);

        for (int i = 0; i < collisionsCheck; i++)
        {
            solarSystem.UpdateBodies(timeStep / collisionsCheck);
            solarSystem.CheckCollisions();
        }

        Camera.main.GetComponent<CameraHandler>().UpdatePosition();
    }

    void SpawnDistantStars()
    {   
        for(int i = 0; i < DistantStarCount; i++){
            Star star = Instantiate(starPrefab);
            star.Initialize();

            float starDist = Distribution.GenerateDistantStarDistance();
            Vector3 pos = Random.onUnitSphere * starDist;

            star.SetValues(pos, new Vector3(0,0,0));
        }
    }

    private void OnValidate()
    {
        bodies = Mathf.Max(0, bodies);
        initialSize = Mathf.Max(0, initialSize);
        timeStep = Mathf.Max(0, timeStep);
        collisionCheckPerTimeStep = Mathf.Max(1, collisionCheckPerTimeStep);
    }
}
