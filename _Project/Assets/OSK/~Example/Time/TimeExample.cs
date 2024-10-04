using System;
using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

public class TimeExample : MonoBehaviour
{
    private Timer myTimer;
    
    [Button("Test Time")]
    private void Test()
    {
        // myTimer = World.Time.Create(gameObject, 2, () =>
        // {
        //     Debug.Log("Hello World");
        // });
        //
        myTimer = Main.Time.Create(gameObject, 1f).Loops(5).Invoke(() => Debug.Log("Tick"));
        // myTimer = World.Time.Create(gameObject, 1f).Unscale().Invoke(() => Debug.Log("Tick"));

    }
    
    [Button("Clear Timers")]
    private void ClearTimers()
    {
        Main.Time.ClearAllTimers();
        Debug.Log("All timers cleared.");
    }
    
    [Button("Paused Timers")]
    private void PausedTimers()
    {
        myTimer.Pause();
        Debug.Log("All timers paused.");
    }
    
    [Button("Resumed Timers")]
    private void ResumedTimers()
    {
        myTimer.Resume();
        Debug.Log("All timers resumed.");
    }

    [Button("Remove Timer")]
    private void RemoveTimer()
    {
        Main.Time.RemoveTimer(myTimer);
        Debug.Log("Timer removed.");
    }
}
