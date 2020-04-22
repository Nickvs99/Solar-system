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
    public void SpawnBodies(int n, float boxWidth)
    {
        for(int i = 0; i < n; i++)
        {
            CelestialBody _body = Instantiate(body);

            float x = Random.Range(0, boxWidth);
            float z = Random.Range(0, boxWidth);

            _body.transform.position = new Vector3(x, 0, z);

            float angle = Random.Range(0, 2 * Mathf.PI);
            float r = Random.Range(0, 0.5f);
            float v_x = Mathf.Cos(angle) * r;
            float v_z = Mathf.Sin(angle) * r;

            _body.velocity = new Vector3(v_x, 0f, v_z);
            bodies.Add(_body);

        }

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

