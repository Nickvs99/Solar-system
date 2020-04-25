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

            // Clamp dist, since collisions have not yet been implemented.
            // This also helps with reducing bodies from being slingshotted away.
            //float distSqr = Mathf.Clamp(vec.sqrMagnitude, 2, Mathf.Infinity);
            float distSqr = vec.sqrMagnitude;
            
            Vector3 dir = vec.normalized;

            Vector3 force = Constants.G * body.mass * this.mass / distSqr * dir;
            Vector3 acc = force / this.mass;

            this.velocity += acc;
        }
    }

   public  void UpdatePosition()
    {
        this.transform.position += this.velocity;
    }
}
