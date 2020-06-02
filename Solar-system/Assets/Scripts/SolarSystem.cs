using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{   
    [SerializeField]
    private Universe universe;
    public List<CelestialBody> bodies  {get; set;} = new List<CelestialBody>();

    public void SpawnBodiesBlock(int n, float boxWidth)
    {
        for(int i = 0; i < n; i++)
        {

            Planet planet = SpawnPlanet();

            float x = Random.Range(0, boxWidth);
            float z = Random.Range(0, boxWidth);

            float angle = Random.Range(0, 2 * Mathf.PI);
            float v_mag = Random.Range(0, boxWidth / 1000f);
            float v_x = Mathf.Cos(angle) * v_mag;
            float v_z = Mathf.Sin(angle) * v_mag;

            planet.SetValues(new Vector3(x, 0, z), new Vector3(v_x, 0f, v_z));
        }

        Camera.main.GetComponent<CameraHandler>().selectedBodies = bodies;
    }
   
    public void SpawnBodiesOrbital()
    {

        Camera.main.GetComponent<CameraHandler>().selectedBodies = bodies;

        SpawnStars();

        SpawnPlanets();

        Debug.LogWarning("In progress");
    }
    public void UpdateBodies(float timeStep)
    {

        foreach (CelestialBody body in bodies)
        {
            body.UpdateVelocity(bodies, timeStep);
        }
        foreach (CelestialBody body in bodies)
        {
            body.UpdatePosition(timeStep);
        }    
    }

    public void ClearBodies()
    {
        foreach(CelestialBody body in bodies)
        {
            Destroy(body.transform.gameObject);
        }
        bodies = new List<CelestialBody>();
    }
    
    public Vector3 CalcCenterOfMass(HashSet<CelestialBody> group)
    {
        Vector3 com = new Vector3(0f, 0f,0f);
        float totalMass = 0;
        foreach(CelestialBody body in group)
        {
            com += body.transform.position * body.Mass;
            totalMass += body.Mass;
        }

        return com / totalMass;
    }

    public void CheckCollisions()
    {
        List<HashSet<CelestialBody>> collisionGroups = GetCollisionGroups();

        //VisualisizeGroups(collisionGroups);
        
        foreach (HashSet<CelestialBody> group in collisionGroups)
        {
            MergeBodies(group);
        }
    }

    /// <summary>
    /// Gets all groups of bodies who are touching eachother
    /// </summary>
    /// <returns></returns>
    private List<HashSet<CelestialBody>> GetCollisionGroups()
    {
        // Init dict,
        // key: a body
        // value: hashset of all the celestialbodies which collide with the key
        IDictionary<CelestialBody, HashSet<CelestialBody>> collisions = new Dictionary<CelestialBody, HashSet<CelestialBody>>();
        foreach (CelestialBody body in bodies)
        {
            collisions.Add(body, new HashSet<CelestialBody>());
        }

        // Get collisions between all bodies
        for (int i = 0; i < bodies.Count; i++)
        {
            for (int j = i + 1; j < bodies.Count; j++)
            {
                if (Vector3.Distance(bodies[i].transform.position, bodies[j].transform.position) < bodies[i].Radius + bodies[j].Radius)
                {
                    collisions[bodies[i]].Add(bodies[j]);
                    collisions[bodies[j]].Add(bodies[i]);
                }
            }
        }

        // Group all bodies who collied with eachother together
        List<HashSet<CelestialBody>> collisionGroups = new List<HashSet<CelestialBody>>();
        HashSet<CelestialBody> checkedBodies = new HashSet<CelestialBody>();
        foreach (KeyValuePair<CelestialBody, HashSet<CelestialBody>> item in collisions)
        {
            if (item.Value.Count == 0 || checkedBodies.Contains(item.Key))
            {
                continue;
            }
            HashSet<CelestialBody> group = GetBodies(item.Key, collisions, checkedBodies);
            collisionGroups.Add(group);
        }

        return collisionGroups;
    }

    /// <summary>
    /// Visual representation of the collision groups. 
    /// </summary>
    /// <param name="collisionGroups"></param>
    private void VisualisizeGroups(List<HashSet<CelestialBody>> collisionGroups)
    {

        foreach(CelestialBody body in bodies)
        {
            body.GetComponent<Renderer>().material.color = Color.white;
        }

        Color[] colors = new Color[5] {
            Color.red, Color.blue, Color.yellow, Color.cyan, Color.magenta
        };

        int index = 0;
        foreach (HashSet<CelestialBody> group in collisionGroups)
        {
            foreach (CelestialBody body in group)
            {
                if (index >= colors.Length)
                {
                    body.GetComponent<Renderer>().material.color = Color.black;
                }
                else
                {
                    body.GetComponent<Renderer>().material.color = colors[index];
                }
            }
            index += 1;
        }
    }

    /// <summary>
    /// Get all the bodies who collied with the body including itself
    /// </summary>
    /// <param name="body"></param>
    /// <param name="collisions"></param>
    /// <param name="checkedBodies"></param>
    /// <returns></returns>
    private HashSet<CelestialBody> GetBodies(CelestialBody body, IDictionary<CelestialBody, HashSet<CelestialBody>> collisions, HashSet<CelestialBody> checkedBodies)
    {
        HashSet<CelestialBody> coll = new HashSet<CelestialBody>() { body };
        checkedBodies.Add(body);

        foreach (CelestialBody other in collisions[body])
        {
            if (checkedBodies.Contains(other))
            {
                continue;
            }

            coll.Add(other);

            HashSet<CelestialBody> col = GetBodies(other, collisions, checkedBodies);

            foreach (CelestialBody temp in col)
            {
                coll.Add(temp);
            }
        }

        return coll;
    }

    /// <summary>
    /// Merges a set of celestial bodies into one body.
    /// </summary>
    /// <param name="group"></param>
    public void MergeBodies(HashSet<CelestialBody> group)
    {
        Vector3 initMomentum = new Vector3(0, 0, 0);
        float totalMass = 0f;
        float avgDensity = 0f;

        Vector3 avgPosition = CalcCenterOfMass(group);

        CelestialBody heaviestBody = GetHeavist(group);

        foreach(CelestialBody body in group)
        {
            totalMass += body.Mass;
            initMomentum += body.Mass * body.Velocity;
            avgDensity += body.Density;
            
            if(body != heaviestBody)
            {
                bodies.Remove(body);
                Destroy(body.gameObject);

                CameraHandler cameraScript = Camera.main.GetComponent<CameraHandler>();
                if (body == cameraScript.CenteredBody)
                {
                    cameraScript.CenteredBody = heaviestBody;
                }

                if (cameraScript.selectedBodies.Contains(body) && !cameraScript.selectedBodies.Contains(heaviestBody))
                {
                    cameraScript.selectedBodies.Add(heaviestBody);
                }
                cameraScript.selectedBodies.Remove(body);
            }
        }

        Vector3 newVelocity = initMomentum / totalMass;
        avgDensity /= group.Count;

        if(heaviestBody is Star)
        {
            Star body = (Star) heaviestBody;
            body.SetValues(avgPosition, newVelocity);
        }
        else if(heaviestBody is Planet)
        {
            Planet body = (Planet) heaviestBody;
            body.SetValues(avgPosition, newVelocity);
        }
    }

    private CelestialBody GetHeavist(HashSet<CelestialBody> groupHashSet)
    {
        List<CelestialBody> group = new List<CelestialBody>(groupHashSet);
        CelestialBody heavistBody = group[0];
        for(int i = 1; i < group.Count; i++)
        {
            if(group[i].Mass > heavistBody.Mass)
            {
                heavistBody = group[i];
            }
        }

        return heavistBody;
    }

    public void SpawnStars(){

        float r = Random.Range(0f,1f);
        if (r < 0.5)
        {
            // Spawn single sun
            
            Star star = SpawnStar();
            star.SetValues(new Vector3(0,0,0), new Vector3(0,0,0));

        } else {
            // Spawn binary system

            Star star1 = SpawnStar();
            Star star2 = SpawnStar();

            float totalRadii = star1.Radius + star2.Radius;
            float dist = Distribution.GenerateDistBinarySystem(totalRadii);

            float totalMass = star1.Mass + star2.Mass;

            float dist1 = dist * star2.Mass / totalMass;
            float dist2 = dist * star1.Mass / totalMass;

            // Calculate the speed of the bodies, derived from keplers third law. Circular orbits
            float v1Mag = Mathf.Sqrt(Mathf.Pow(star2.Mass, 3) * Constants.G / (Mathf.Pow(totalMass, 2) * dist1));
            float v2Mag = Mathf.Sqrt(Mathf.Pow(star1.Mass, 3) * Constants.G / (Mathf.Pow(totalMass, 2) * dist2));

            Vector3 v1 = new Vector3(0,0, v1Mag);
            Vector3 v2 = new Vector3(0,0, -v2Mag);

            star1.SetValues(new Vector3(dist1, 0 , 0), v1);    
            star2.SetValues(new Vector3(-dist2, 0 , 0), v2);

        }
    }

    public void SpawnPlanets(){

        float distFromOrigin = GetMaxSemiMajorAxisSuns();

        float sunMasses = GetTotalSunMass();
        int n = 0;
        while(n < 2 || Random.Range(0f,1f) < 0.6f){
            
            Planet planet = SpawnPlanet();

            distFromOrigin += Distribution.GenerateSemiMajorAddition();

            float theta = Random.Range(0,360f);

            float x = Mathf.Cos(theta) * distFromOrigin;
            float z = Mathf.Sin(theta) * distFromOrigin;

            Vector3 pos = new Vector3(x, 0, z);

            float vMag = CalcOrbitalVelocity(distFromOrigin, sunMasses);

            Vector3 vel = Vector3.Cross(pos, new Vector3(0,1,0)).normalized * vMag;

            planet.SetValues(pos, vel);
            n++;
        }
    }

    public float GetTotalSunMass(){
        float totalMass = 0;
        foreach(CelestialBody body in bodies){
            totalMass += body.Mass;
        }
        return totalMass;
    }

    public float GetMaxSemiMajorAxisSuns(){

        float[] semiMajorAxises = new float[bodies.Count];

        for(int i = 0; i< bodies.Count; i++){

            semiMajorAxises[i] = Vector3.Distance(new Vector3(0,0,0), bodies[i].transform.position);
        }

        return Mathf.Max(semiMajorAxises);
    }

    private Star SpawnStar(){
        Star star = Instantiate(universe.starPrefab);
        star.Initialize();
        bodies.Add(star);
        return star;
    }

    private Planet SpawnPlanet(){
        Planet planet = Instantiate(universe.planetPrefab);
        planet.Initialize();
        bodies.Add(planet);
        return planet;
    }

    public float CalcOrbitalVelocity(float dist, float bigMass)
    {
        return Mathf.Sqrt(Constants.G * bigMass / dist);
    }

    public void OnDrawGizmos(){
        foreach(CelestialBody body in bodies){
            Vector3 pos = body.transform.position;
            if(body is Star){
                Gizmos.color = Color.yellow;
            }
            else {
                Gizmos.color = Color.gray;
            }
            Gizmos.DrawSphere(pos, Utility.GizmoRadius(pos, 0.025f));
        }
    }

}

