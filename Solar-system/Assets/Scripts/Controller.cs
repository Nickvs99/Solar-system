using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private CameraHandler cameraScript;

    public Universe universe;

    void Start()
    {
        cameraScript = Camera.main.GetComponent<CameraHandler>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            cameraScript.SetMouseDown();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            cameraScript.UpdateSelectedBodies();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            universe.RespawnSystem();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            universe.FlipPlayState();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            universe.ManualUpdate();
        }
    }
}
