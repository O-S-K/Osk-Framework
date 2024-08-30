using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingExample : MonoBehaviour
{
    private float percentage = 0;
    
    private void Start()
    {
        // scene loading example
        World.Scene.OnLoadingStart += OnLoadingStart;
        World.Scene.OnLoadingProgress += OnLoadingProgress;
        World.Scene.OnLoadingComplete += OnLoadingComplete;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            World.Scene.LoadSceneAsync("Gameplay", LoadSceneMode.Single);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            World.Scene.UnloadScene("Gameplay");
        }
    }

    private void OnLoadingStart()
    {
        Debug.Log("Loading Start");
    }
    
    private void OnLoadingProgress(float progress)
    {
        percentage += progress;
        Debug.Log("Loading Progress -> " + percentage + "%");
    }
    
    private void OnLoadingComplete()
    {
        Debug.Log("Loading Complete");
    }
    
    
}