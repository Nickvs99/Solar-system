﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    private CameraHandler cameraScript;
    public Universe universe;

    private float startClickTime;

    private float HOLD_TIME = 0.2f;

    void Start()
    {
        cameraScript = Camera.main.GetComponent<CameraHandler>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            cameraScript.SetMouseDown();
            startClickTime = Time.time;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            float endClickTime = Time.time;
            float deltaTime = endClickTime - startClickTime;

            if(deltaTime < HOLD_TIME)
            {
                cameraScript.HighlightBody();
            }
            else
            {
                cameraScript.UpdateSelectedBodies();
            }
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