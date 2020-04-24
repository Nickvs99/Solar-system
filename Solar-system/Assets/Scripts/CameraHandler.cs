using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public Universe universe;

    void Update()
    {
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

        foreach (CelestialBody body in universe.solarSystem.bodies)
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
    }
}
