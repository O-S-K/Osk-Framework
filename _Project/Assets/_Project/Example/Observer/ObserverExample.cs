using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverExample : MonoBehaviour
{
    private int score = 0;
    private void Start()
    {
        World.Observer.Add("UpdateScore", OnUpdateScore);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            World.Observer.Notify("UpdateScore", 1);
        }
    }

    private void OnUpdateScore(object data)
    {
        score += (int)data;
        Debug.Log("Score Updated! ->  " + score);
    } 

    private void OnDestroy()
    {
        World.Observer.Remove("UpdateScore", OnUpdateScore);
    }
}
