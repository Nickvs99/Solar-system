using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    public CelestialBody bodyPrefab;
    public List<CelestialBody> bodies = new List<CelestialBody>();

    private void Awake()
    {
        bodies = new List<CelestialBody>();
    }
    public void SpawnBodies(int n, float boxWidth)
    {
        for(int i = 0; i < n; i++)
        {
            CelestialBody _body = Instantiate(bodyPrefab);

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

    public void ClearBodies()
    {
        foreach(CelestialBody body in bodies)
        {
            Destroy(body.transform.gameObject);
        }
        bodies = new List<CelestialBody>();
    }

    public Vector3 CalcCenterOfMass()
    {
        Vector3 com = new Vector3(0f, 0f,0f);
        float totalMass = 0;
        foreach(CelestialBody body in bodies)
        {
            com += body.transform.position * body.mass;
            totalMass += body.mass;
        }

        return com / totalMass;
    }

    public void CheckCollisions()
    {
        List<HashSet<CelestialBody>> collisionGroups = GetCollisionGroups();

        foreach(HashSet<CelestialBody> group in collisionGroups)
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
                // TODO make the dist cutoff based on the size of the body
                if (Vector3.Distance(bodies[i].transform.position, bodies[j].transform.position) < 1f)
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
            item.Key.GetComponent<Renderer>().material.color = Color.red;

            HashSet<CelestialBody> group = GetBodies(item.Key, collisions, checkedBodies);
            collisionGroups.Add(group);
        }

        VisualisizeGroups(collisionGroups);

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

    /// Get all the bodies who collied with the body including itself
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
        Vector3 pInitial = new Vector3(0, 0, 0);
        Vector3 posAvg = new Vector3(0, 0, 0);
        float totalMass = 0f;

        foreach(CelestialBody body in group)
        {
            totalMass += body.mass;
            pInitial += body.mass * body.velocity;
            posAvg += body.transform.position;

            bodies.Remove(body);
            Destroy(body.gameObject);
        }


        Vector3 v_new = pInitial / totalMass;
        posAvg /= group.Count;

        CelestialBody bodyNew = Instantiate(bodyPrefab);
        bodyNew.transform.position = posAvg;
        bodyNew.mass = totalMass;
        bodyNew.velocity = v_new;
        bodies.Add(bodyNew);

    }
}

