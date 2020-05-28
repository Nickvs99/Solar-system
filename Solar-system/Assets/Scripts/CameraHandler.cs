using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CameraHandler : MonoBehaviour
{
    public Universe universe;

    public List<CelestialBody> selectedBodies {get; set;}

    private CelestialBody centeredBody;
    public CelestialBody CenteredBody {
        get {
            return centeredBody;
        }
        set {
            centeredBody = value;
            if (value != null){
                fixedToOrigin = false;  
            }
        }
    }    
    
    public bool fixedToOrigin {get; set;} = false;
    public Vector3 mouseDown;
    private Vector3 mouseUp;
    public Vector3 MouseUp {
        get {
            return mouseUp;
        }
        set {
            mouseUp = value;
            selectedBodies = GetBodiesInRange(mouseDown, mouseUp);
            fixedToOrigin = false;
        }
    }
    public bool fixedHeight {get; set;} = false;

    public void LateUpdate(){
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        this.transform.position = CalcPosition();
    }

    public void SetMouseDown()
    {
        mouseDown = GetIntersectXZPlane();
    }
    public void SetMouseUp(){
        MouseUp = GetIntersectXZPlane();
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

        float xPos, zPos;
        if(fixedToOrigin){
            xPos = 0;
            zPos = 0;
        }
        else if (centeredBody == null)
        {
            // Middle of all the bodies
            xPos = (coor[1] + coor[3]) / 2;
            zPos = (coor[0] + coor[2]) / 2;
        }
        else
        {
            xPos = centeredBody.transform.position.x;
            zPos = centeredBody.transform.position.z;
        }

        float height;
        if (fixedHeight)
        {
            height = this.transform.position.y;
        }
        else
        {
            float upDist = coor[0] - zPos;
            float rightDist = coor[1] - xPos;
            float downDist = zPos - coor[2];
            float leftDist = xPos - coor[3];

            // Get the max vertical and horizontal dist
            float xDist = Mathf.Max(rightDist, leftDist);
            float zDist = Mathf.Max(upDist, downDist);

            // Get the height only looking at either the x or z coordinates
            float heightX = xDist / (Mathf.Tan(fovHorizontal / 2));
            float heightZ = zDist / (Mathf.Tan(fovVertical / 2));

            // Get the max height and at a little margin
            height = Mathf.Max(heightX, heightZ) * 1.3f;
        }

        // Minimal value, this removes the spasm when the camera is very
        // close to the celestial bodies.
        float minHeight = CalcMinHeight();
        height = Mathf.Max(minHeight, height);

        return new Vector3(xPos, height, zPos);
    }

    private float CalcMinHeight(){
        float maxRadius = GetMaxRadius();

        float nearField = Camera.main.nearClipPlane;

        return Mathf.Max(maxRadius, nearField) * 1.3f;
    }

    private float GetMaxRadius(){
        float maxRadius = 0f;
        foreach(CelestialBody body in selectedBodies){
            if(body.Radius > maxRadius){
                maxRadius = body.Radius;
            }
        }
        return maxRadius;
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
            float r = body.Radius;

            float xCorRight = body.transform.position.x + r;
            float xCorLeft = body.transform.position.x - r;
            float zCorTop = body.transform.position.z + r;
            float zCorBottom = body.transform.position.z - r;

            if (xCorLeft < leftCor)
            {
                leftCor = xCorLeft;
            }

            if (xCorRight > rightCor)
            {
                rightCor = xCorRight;
            }

            if (zCorBottom < bottomCor)
            {
                bottomCor = zCorBottom;
            }

            if (zCorTop > topCor)
            {
                topCor = zCorTop;
            }
        }

        return new float[] { topCor, rightCor, bottomCor, leftCor };
    }

    private void OnDrawGizmos()
    {
        float[] coor = CalcBoundaries();

        Vector3[] corners = new Vector3[4]{

            new Vector3(coor[1], 0, coor[0]),       //top right
            new Vector3(coor[1], 0, coor[2]),       //bottom right
            new Vector3(coor[3], 0, coor[2]),       //bottom left
            new Vector3(coor[3], 0, coor[0])        //top left
        };

        foreach(Vector3 corner in corners){
            Gizmos.DrawSphere(corner, Utility.GizmoRadius(corner, 0.01f));
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

    public void HighlightBody()
    { 
        Vector3 intersect = GetIntersectXZPlane();

        foreach(CelestialBody body in selectedBodies)
        {
            if(Vector3.Distance(body.transform.position, intersect) < body.Radius)
            {
                Selection.activeGameObject = body.gameObject;
                return;
            }
        }
    }

    public void SetCenterBody()
    {
        Vector3 intersect = GetIntersectXZPlane();

        foreach (CelestialBody body in selectedBodies)
        {
            if (Vector3.Distance(body.transform.position, intersect) < body.Radius)
            {
                // Reset centered body
                if (body == centeredBody)
                {
                    centeredBody = null;
                }
                else
                {
                    centeredBody = body;
                    fixedToOrigin = false;
                }

                return;
            }
        }
    }

    public void ResetSelectedBodies()
    {
        selectedBodies = universe.solarSystem.bodies;
    }

    public void FlipFixedHeight()
    {
        fixedHeight = !fixedHeight;
    }

    public void SetOrigin(){
        fixedToOrigin = true;
        centeredBody = null;
    }
}
