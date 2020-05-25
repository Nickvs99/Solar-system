using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {
    
    public static float GizmoRadius(Vector3 r, float perc){
        
        Vector3 cameraPosition = Camera.main.transform.position;
        float fov = Camera.main.fieldOfView * Mathf.Deg2Rad;

        return perc * Vector3.Distance(cameraPosition, r) * Mathf.Tan(fov/2);
    }
}