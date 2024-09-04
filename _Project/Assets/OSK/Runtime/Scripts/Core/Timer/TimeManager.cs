using System;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

public class TimeManager : GameFrameworkComponent
{
    // List to keep track of all active timers
    [ShowInInspector]
    private List<Timer> timers = new List<Timer>();


    // Method to create a new timer
    public Timer Create(GameObject owner, float duration, Action onDispose = null)
    {
        Timer newTimer = new Timer();
        newTimer.Start(owner, duration, onDispose);
        timers.Add(newTimer);
        return newTimer;
    }
    

    // Method to update all timers (called in FixedUpdate)
    private void FixedUpdate()
    {
        for (int i = timers.Count - 1; i >= 0; i--)
        {
            Timer timer = timers[i];
            timer.FixedUpdate();

            // Remove the timer if it has been disposed
            if (timer == null || timer.Equals(null))
            {
                timers.RemoveAt(i);
            }
        }
    }

    // method to pause all timers
    public void PauseAllTimers()
    {
        foreach (Timer timer in timers)
        {
            timer.Pause();
        }
    }

    // method to resume all timers
    public void ResumeAllTimers()
    {
        foreach (Timer timer in timers)
        {
            timer.Resume();
        }
    }

    // Method to remove a specific timer
    public void RemoveTimer(Timer timer)
    {
        if (timers.Contains(timer))
        {
            timer.Dispose();
            timers.Remove(timer);
        }
    }

    // Method to clear all timers
    public void ClearAllTimers()
    {
        foreach (Timer timer in timers)
        {
            timer.Dispose();
        }

        timers.Clear();
    }
}