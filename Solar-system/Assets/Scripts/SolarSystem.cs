using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    public CelestialBody body;
    List<CelestialBody> bodies = new List<CelestialBody>();

    private void Awake()
    {
        bodies = new List<CelestialBody>();
    }
    public void SpawnBodies()
    {
        // Instantiate N bodies
        CelestialBody body1 = Instantiate(body);
        body1.transform.position = new Vector3(-2f, 0f, 0f);
        body1.velocity = new Vector3(0f, 0f, 0.1f);
        bodies.Add(body1);

        CelestialBody body2 = Instantiate(body);
        body2.transform.position = new Vector3(2f, 0f, 0f);
        body2.velocity = new Vector3(0f, 0f, -0.1f);
        bodies.Add(body2);

        CelestialBody body3 = Instantiate(body);
        body3.transform.position = new Vector3(0f, 0f, 8f);
        body3.velocity = new Vector3(0.1f, 0f, 0.2f);
        bodies.Add(body3);

    }

    
    public void UpdateBodies()
    {

        foreach (CelestialBody body in bodies)
        {
            body.UpdateVelocity(bodies);
        }
        foreach (CelestialBody body in bodies)
        {
            body.UpdatePosition();
        }    
    }
}

