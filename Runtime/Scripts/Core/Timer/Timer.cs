using System;
using CustomInspector;
using UnityEngine;

namespace OSK
{
    public sealed class Timer
    {
        [SerializeField] internal int count;
        [SerializeField] internal bool unscale;
        [SerializeField] internal bool isPaused;
        [SerializeField] internal float interval;
        
        [SerializeField] internal float multiplier = 1;
        [SerializeField] internal float timePerSecond = 1;
        
        [SerializeField] internal float duration;
        [SerializeField] internal float pausedTime;
        [SerializeField] internal bool useFixedUpdate;
        [SerializeField] internal bool isTimeOut;
        [SerializeField] internal GameObject owner;

        public Action OnStart;
        public Action OnTick;
        public Action OnFixedTick;
        public Action OnTickPerSecond;
        public Action OnCompleted;

        internal Action OnPause;
        internal Action OnResume;

        // Use the correct time depending on unscale value
        private float fixedSeconds => (unscale ? Time.fixedUnscaledTime : Time.fixedTime) * multiplier;
        private float frameSeconds => (unscale ? Time.unscaledTime : Time.time) * multiplier;
        public int Second => (int)interval;

        // not use CreateLoops
        public Timer SetTimeOut(float duration)
        {
            this.duration = duration;
            interval = (useFixedUpdate ? fixedSeconds : frameSeconds) + duration;
            return this;
        }

        // not use CreateLoops
        public Timer Add(float interval)
        {
            this.interval += interval * multiplier;
            return this;
        }

        // Use -1 for infinite loops not use CreateLoops
        public Timer Loops(int count = 0)
        {
            this.count = count;
            if (count < 0)
                SetTimeOut(0);
            return this;
        }

        public Timer UseFixedUpdate(bool isFixedUpdate)
        {
            this.useFixedUpdate = isFixedUpdate;
            return this;
        }

        public Timer SetSpeedTime(float multiplier)
        {
            this.multiplier = multiplier;
            return this;
        }

        public Timer Unscale(bool unScale = true)
        {
            this.unscale = unScale;
            interval = (useFixedUpdate ? fixedSeconds : frameSeconds) + duration;
            return this;
        }

        public void Dispose()
        {
            owner = null;
            OnStart = null;
            OnTick = null;
            OnFixedTick = null;
            isTimeOut = true;
            OnCompleted?.Invoke();
            OnCompleted = null;
            Main.Time.UpdateListTimer();
        }

        internal Timer Start(GameObject owner)
        {
            count = 1;
            unscale = false;
            isPaused = false;
            isTimeOut = false;
            this.owner = owner;
            return this;
        }

        public void Pause()
        {
            if (!isPaused)
            {
                OnPause?.Invoke();
                isPaused = true;
                pausedTime = useFixedUpdate ? fixedSeconds : frameSeconds;
            }
        }

        public void Resume()
        {
            if (isPaused)
            {
                OnResume?.Invoke();
                isPaused = false;
                // Adjust interval to account for paused time
                interval += (useFixedUpdate ? fixedSeconds : frameSeconds) - pausedTime;
            }
        }

        internal void FixedUpdate()
        {
            HandleTimer(fixedSeconds, useFixedUpdate);
        }

        internal void Update()
        {
            HandleTimer(frameSeconds, useFixedUpdate);
        }

        private void HandleTimer(float currentTime, bool isFixedUpdate)
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

            if (timePerSecond <= 0)
            {
                timePerSecond = 1f;
                OnTickPerSecond?.Invoke();
            }
            else
            {
                timePerSecond -= (isFixedUpdate ? Time.fixedDeltaTime : Time.deltaTime) * multiplier;
            }

            count--;
            interval = currentTime + duration * multiplier;
            if (isFixedUpdate)
            {
                OnFixedTick?.Invoke();
            }
            else
            {
                OnTick?.Invoke();
            }

            if (count == 0)
            {
                //Logg.Log("Timer disposed.");
                Dispose();
            }
        }
    }
}