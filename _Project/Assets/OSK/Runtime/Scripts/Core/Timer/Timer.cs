using System;
using UnityEngine;

public sealed class Timer
{
    private int count;
    private bool unscale;
    private bool isPaused;
    private float interval;
    private float duration;
    private float pausedTime;
    private GameObject owner;
    private event Action OnUpdate;
    private event Action OnDispose;

    private float seconds => unscale ? Time.fixedUnscaledTime : Time.fixedTime;

    public Timer Invoke(Action OnUpdate)
    {
        this.OnUpdate = OnUpdate;
        return this;
    }

    public Timer Set(float duration)
    {
        this.duration = duration;
        interval = seconds + duration;
        return this;
    }

    public Timer Add(float interval)
    {
        this.interval += interval;
        return this;
    }

    public Timer Loops(int count = 0)
    {
        this.count = count;
        return this;
    }

    public Timer Unscale(bool unscale = true)
    {
        this.unscale = unscale;
        interval = seconds + duration;
        return this;
    }

    public void Dispose()
    {
        owner = null;
        OnUpdate = null;
        OnDispose?.Invoke();
        OnDispose = null;
    }

    internal void Start(GameObject owner, float duration, Action OnDispose)
    {
        count = 1;
        unscale = false;
        isPaused = false;
        this.owner = owner;
        this.duration = duration;
        interval = seconds + duration;
        this.OnDispose = OnDispose;
    }

    public void Pause()
    {
        if (!isPaused)
        {
            isPaused = true;
            pausedTime = seconds;
        }
    }

    public void Resume()
    {
        if (isPaused)
        {
            isPaused = false;
            interval += seconds - pausedTime;
        }
    }

    internal void FixedUpdate()
    {
        try
        {
            if (owner == null)
            {
                Dispose();
                return;
            }

            if (isPaused)
            {
                return;
            }

            if (seconds <= interval)
            {
                return;
            }

            count--;
            interval = seconds + duration;
            OnUpdate?.Invoke();
            if (count == 0)
            {
                Debug.Log("Timer disposed.");
                Dispose();
            }
        }
        catch (Exception e)
        {
            Dispose();
            Debug.LogError("Timer encountered an error: " + e.Message);
        }
    }
}