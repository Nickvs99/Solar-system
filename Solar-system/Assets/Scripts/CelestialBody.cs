using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    [SerializeField]
    private float temp;

    // NOTE The use of auto-implemented properies are not used, since they won't
    // show up in the inspector
    [SerializeField]
    private Vector3 velocity;
    public Vector3 Velocity 
    {
        get {
            return velocity;
        } 
        set {
            velocity = value;
        }
    }

    [SerializeField]
    private float mass;
    public float Mass 
    {
        get {
            return mass;
        } 
        set {
            mass = value;

            if(density !=  0){
                radius = CalcRadius(Mass, Density);
                float diameter = 2 * Radius;
                transform.localScale = new Vector3(diameter, diameter, diameter);
            }
        }
    }

    [SerializeField]
    private float density;
    public float Density 
    {
        get {
            return density;
        } set{
            density = value;

            if(Radius != 0f){
                mass = CalcMass(Density, Radius);
            }
        }
    }

    [SerializeField]
    private float radius;
    public float Radius
    {
        get {
            return radius;
        }
        set {
            radius = value;

            if(Mass != 0f){

                density = CalcDensity(Mass, Radius);

                float diameter = 2 * Radius;
                transform.localScale = new Vector3(diameter, diameter, diameter);
            }
        }
    }

    public void SetValues(Vector3 _position, Vector3 _velocity)
    {
        this.transform.position = _position;
        Velocity = _velocity;
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

            Vector3 force = Constants.G * body.Mass * this.Mass / distSqr * dir;
            Vector3 acc = force / this.Mass;

            this.Velocity += acc * timeStep;
        }
    }

   public  void UpdatePosition(float timeStep)
    {
        this.transform.position += this.Velocity * timeStep;
    }

    public float CalcRadius(float mass, float density)
    {         
        return Mathf.Pow(mass * 3 / 4 / Mathf.PI / density, 1f/3);
    }

    public float CalcMass(float density, float radius)
    {
        return 4f / 3 * Mathf.PI * Mathf.Pow(radius, 3) * density;
    }
    public float CalcDensity(float mass, float radius)
    {
        return mass / (4f / 3 * Mathf.PI * Mathf.Pow(radius, 3));
    }
}
