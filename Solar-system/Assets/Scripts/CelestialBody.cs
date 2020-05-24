using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    public Vector3 velocity;
    public float mass;
    public float density;
    public float radius;

    public void SetValues(Vector3 _position, Vector3 _velocity)
    {
        this.transform.position = _position;
        velocity = _velocity;
    }

    public void UpdateVelocity(List<CelestialBody> bodies, float timeStep)
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

            this.velocity += acc * timeStep;
        }
    }

   public  void UpdatePosition(float timeStep)
    {
        this.transform.position += this.velocity * timeStep;
    }

    public float CalcRadius(float mass, float density)
    {
        // M = 4/3 Pi r^3 * dens => r = (m * 3/ 4 / Pi / dens) ** 1/3
        float radius = Mathf.Pow(mass * 3 / 4 / Mathf.PI / density, 1f/3);        
        
        // Temp, will be updated once I understand get/set
        float diameter = 2 * radius;
        transform.localScale = new Vector3(diameter, diameter, diameter);
        
        return radius;
    }
}
