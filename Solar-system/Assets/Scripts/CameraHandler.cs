﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public Universe universe;
    public List<CelestialBody> selectedBodies;

    private Vector3 mouseDown;
    private Vector3 mouseUp;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            mouseDown = GetIntersectXZPlane();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            mouseUp = GetIntersectXZPlane();

            selectedBodies = GetBodiesInRange(mouseDown, mouseUp);
        }

        Vector3 pos = CalcPosition();
        this.transform.position = pos; 
    }

    /// <summary>
    /// Calculates the position of the camera. At this position the camera can see 
    /// all celestial bodies from a birds eye view.
    /// </summary>
    /// <returns>Vector3</returns>
    Vector3 CalcPosition()
    {
        float[] coor = CalcBoundaries();

        float fovVertical = Camera.main.fieldOfView * Mathf.Deg2Rad;

        //https://en.wikipedia.org/wiki/Field_of_view_in_video_games#Field_of_view_calculations
        float fovHorizontal = 2 * Mathf.Atan(Mathf.Tan(fovVertical / 2) * Camera.main.aspect);

        float xDist = coor[1] - coor[3];
        float zDist = coor[0] - coor[2];

        float xAvg = (coor[1] + coor[3]) / 2;
        float zAvg = (coor[0] + coor[2]) / 2;
    
        float heightX = xDist / (2 * Mathf.Tan(fovHorizontal / 2)) ;
        float heightZ = zDist / (2 * Mathf.Tan(fovVertical / 2));

        // Get the max height and at a little margin
        float height = Mathf.Max(heightX, heightZ) * 1.3f;

        // Minimal value, this removes the spasm when the camera is very
        // close to the celestial bodies.
        height = Mathf.Max(10f, height);

        return new Vector3(xAvg, height, zAvg);
    }

    
    /// <summary>
    /// Calculates the boundaries of a rectangle spanned by the celestial body. 
    /// </summary>
    /// <returns>float[]: top, right, bottom, left</returns>
    float[] CalcBoundaries()
    {
        float leftCor = Mathf.Infinity;
        float rightCor = -Mathf.Infinity;
        float topCor = -Mathf.Infinity;
        float bottomCor = Mathf.Infinity;

        foreach (CelestialBody body in selectedBodies)
        {
            float xCor = body.transform.position.x;
            float zCor = body.transform.position.z;

            if (xCor < leftCor)
            {
                leftCor = xCor;
            }

            if (xCor > rightCor)
            {
                rightCor = xCor;
            }

            if (zCor < bottomCor)
            {
                bottomCor = zCor;
            }

            if (zCor > topCor)
            {
                topCor = zCor;
            }
        }

        return new float[] { topCor, rightCor, bottomCor, leftCor };
    }

    private void OnDrawGizmos()
    {
        float[] coor = CalcBoundaries();

        Vector3 topRight = new Vector3(coor[1], 0, coor[0]);
        Vector3 bottomRight = new Vector3(coor[1], 0, coor[2]);
        Vector3 bottomLeft = new Vector3(coor[3], 0, coor[2]);
        Vector3 topLeft = new Vector3(coor[3], 0, coor[0]);

        Gizmos.DrawSphere(topRight, 1);
        Gizmos.DrawSphere(bottomRight, 1);
        Gizmos.DrawSphere(bottomLeft, 1);
        Gizmos.DrawSphere(topLeft, 1);

        if (mouseDown != null && mouseUp != null)
        {
            Gizmos.DrawSphere(mouseDown, 2);
            Gizmos.DrawSphere(mouseUp, 2);
        }
    }

    /// <summary>
    /// Calculates and returns the intersections between the mouse and the xz-plane
    /// </summary>
    /// <returns></returns>
    private Vector3 GetIntersectXZPlane()
    {
        Vector3 mouse = Input.mousePosition;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, 10f));
        Vector3 dir = worldPoint - transform.position;

        // camera.y - alpha * dir.y = 0. Solve for alpha
        float alpha = transform.position.y / dir.y;

        return transform.position - alpha * dir;
    }

    /// <summary>
    /// Get all bodies within the x and z range set by two vectors
    /// </summary>
    /// <param name="vector1"></param>
    /// <param name="vector2"></param>
    /// <returns></returns>
    private List<CelestialBody> GetBodiesInRange(Vector3 vector1, Vector3 vector2)
    {
        float minX = Mathf.Min(vector1.x, vector2.x);
        float maxX = Mathf.Max(vector1.x, vector2.x);
        float minZ = Mathf.Min(vector1.z, vector2.z);
        float maxZ = Mathf.Max(vector1.z, vector2.z);

        List<CelestialBody> validBodies = new List<CelestialBody>();
        foreach(CelestialBody body in universe.solarSystem.bodies)
        {
            float bodyX = body.transform.position.x;
            float bodyZ = body.transform.position.z;

            if(bodyX > minX && bodyX < maxX && bodyZ > minZ && bodyZ < maxZ)
            {
                validBodies.Add(body);
            }
        }

        // If no bodies are selected, return the current selected bodies
        if(validBodies.Count == 0)
        {
            return selectedBodies;
        }

        return validBodies;
    }
}
