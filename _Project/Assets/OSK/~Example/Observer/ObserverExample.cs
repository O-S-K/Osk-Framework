using System;
using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;

public class ObserverExample : MonoBehaviour
{
    private int score = 0;
    private string keyObserver = "UpdateScore";
    
    private void Start()
    {
        Main.Observer.Add(keyObserver, OnUpdateScore);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Main.Observer.Notify(keyObserver, 1);
        }
    }

    private void OnUpdateScore(object data)
    {
        score += (int)data;
        Debug.Log("Score Updated! ->  " + score);
    } 

    private void OnDestroy()
    {
        Main.Observer.Remove(keyObserver, OnUpdateScore);
    }
}
