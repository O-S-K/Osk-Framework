using System;
using CustomInspector;
using UnityEngine;

public sealed class Timer
{
    public int count;
    public bool unscale;
    public bool isPaused;
    public float interval;
    public float duration;
    public float pausedTime;
    public GameObject owner;

    internal Action OnStart;
    internal Action OnTick;
    internal Action OnExit;

    internal Action OnPause;
    internal Action OnResume;

    // Use the correct time depending on unscale value
    private float fixedSeconds => unscale ? Time.fixedUnscaledTime : Time.fixedTime;
    private float frameSeconds => unscale ? Time.unscaledTime : Time.time;

    public Timer Invoke(Action OnUpdate)
    {
        this.OnTick = OnUpdate;
        return this;
    }

    public Timer Set(float duration)
    {
        this.duration = duration;
        interval = frameSeconds + duration; // Initialize based on Update logic
        return this;
    }

    public Timer Add(float interval)
    {
        this.interval += interval;
        return this;
    }

    // Use -1 for infinite loops
    public Timer Loops(int count = 0)
    {
        this.count = count;
        return this;
    }


    public Timer Unscale(bool unScale = true)
    {
        this.unscale = unScale;
        interval = frameSeconds + duration;
        return this;
    }

    public void Dispose()
    {
        World.Time.UpdateListTimer();
        owner = null;
        OnTick = null;
        OnExit?.Invoke();
        OnExit = null;
    }

    internal void Start(GameObject owner, float duration, Action OnStart = null, Action OnExit = null)
    {
        count = 1;
        unscale = false;
        isPaused = false;
        this.owner = owner;
        this.duration = duration;
        interval = frameSeconds + duration; // Set initial interval for Update loop
        this.OnStart = OnStart;
        this.OnExit = OnExit;
    }

    internal void Start(GameObject owner, float duration)
    {
        count = 1;
        unscale = false;
        isPaused = false;
        this.owner = owner;
        this.duration = duration;
        interval = frameSeconds + duration; // Set initial interval for Update loop
    }

    public void Pause()
    {
        if (!isPaused)
        {
            OnPause?.Invoke();
            isPaused = true;
            pausedTime = frameSeconds; // Pause based on current time (Update logic)
        }
    }

    public void Resume()
    {
        if (isPaused)
        {
            OnResume?.Invoke();
            isPaused = false;
            interval += frameSeconds - pausedTime; // Adjust interval to account for paused time
        }
    }

    internal void FixedUpdate()
    {
        HandleTimer(fixedSeconds);
    }

    internal void Update()
    {
        HandleTimer(frameSeconds);
    }

    // Handle timer logic shared between FixedUpdate and Update
    private void HandleTimer(float currentTime)
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

            if (currentTime <= interval)
            {
                return;
            }

            count--;
            interval = currentTime + duration;
            OnTick?.Invoke();

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