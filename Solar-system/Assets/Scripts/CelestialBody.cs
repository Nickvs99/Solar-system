using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    public Vector3 velocity;
    public float mass = 1;

    public void UpdateVelocity(List<CelestialBody> bodies)
    {
        foreach (CelestialBody body in bodies)
        {
            if (body == this)
            {
                continue;
            }

            Vector3 vec = body.transform.position - this.transform.position;
            float distSqr = vec.sqrMagnitude;
            Vector3 dir = vec.normalized;

            Vector3 force = Constants.G * body.mass * body.mass / distSqr * dir;
            Vector3 acc = force / body.mass;

            this.velocity += acc;

        }
    }

   public  void UpdatePosition()
    {
        this.transform.position += this.velocity;
    }
}
