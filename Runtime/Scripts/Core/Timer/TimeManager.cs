using System;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

namespace OSK
{
    public class TimeManager : GameFrameworkComponent
    {
        // Lists to keep track of all active timers
        [ShowInInspector, ReadOnly] private List<Timer> updateTimers = new List<Timer>();
        [ShowInInspector, ReadOnly] private List<Timer> fixedUpdateTimers = new List<Timer>();

        // update every frame for a duration
        public Timer Create(MonoBehaviour owner, float duration, bool useFixedUpdate = false, Action onDispose = null)
        {
            return Create(owner.gameObject, duration, useFixedUpdate, onDispose);
        }

        public Timer Create(GameObject owner, float duration, bool useFixedUpdate = false, Action onDispose = null)
        {
            Timer newTimer = new Timer();
            newTimer.Start(owner, duration, onDispose);

            if (useFixedUpdate)
            {
                fixedUpdateTimers.Add(newTimer);
            }
            else
            {
                updateTimers.Add(newTimer);
            }

            return newTimer;
        }
        
        public DateTime GetRealTimeWorld()
        {
            WorldTimeAPI worldTimeAPI = new WorldTimeAPI();
            return worldTimeAPI.GetCurrentDateTime();
        }
         

        // update every frame
        public Timer Create(MonoBehaviour owner, bool useFixedUpdate = false)
        {
            Timer newTimer = new Timer();
            newTimer.Start(owner.gameObject, 0);
            newTimer.Loops(-1);

            if (useFixedUpdate)
            {
                fixedUpdateTimers.Add(newTimer);
            }
            else
            {
                updateTimers.Add(newTimer);
            }

            return newTimer;
        }

        public Timer Create(GameObject owner, bool useFixedUpdate = false)
        {
            Timer newTimer = new Timer();
            newTimer.Start(owner, 0);
            newTimer.Loops(-1);

            if (useFixedUpdate)
            {
                fixedUpdateTimers.Add(newTimer);
            }
            else
            {
                updateTimers.Add(newTimer);
            }

            return newTimer;
        }

        public void AddSpeedMultiplier(float speedMultiplier)
        {
            foreach (Timer timer in updateTimers)
            {
                timer.AddSpeedMultiplier(speedMultiplier);
            }

            foreach (Timer timer in fixedUpdateTimers)
            {
                timer.AddSpeedMultiplier(speedMultiplier);
            }
        }

        // Method to update all timers (called in Update)
        private void Update()
        {
            for (int i = updateTimers.Count - 1; i >= 0; i--)
            {
                Timer timer = updateTimers[i];
                timer.Update();

                // Remove the timer if it has been disposed
                if (timer == null || timer.Equals(null))
                {
                    updateTimers.RemoveAt(i);
                }
            }
        }

        // Method to update all timers (called in FixedUpdate)
        private void FixedUpdate()
        {
            for (int i = fixedUpdateTimers.Count - 1; i >= 0; i--)
            {
                Timer timer = fixedUpdateTimers[i];
                timer.FixedUpdate();

                // Remove the timer if it has been disposed
                if (timer == null || timer.Equals(null))
                {
                    fixedUpdateTimers.RemoveAt(i);
                }
            }
        }

        // Method to pause all timers
        public void PauseTimer(Timer timer)
        {
            timer.Pause();
        }

        [Button]
        public void PauseAllTimers()
        {
            foreach (Timer timer in updateTimers)
            {
                timer.Pause();
            }

            foreach (Timer timer in fixedUpdateTimers)
            {
                timer.Pause();
            }
        }

        // Method to resume all timers
        public void ResumeTimer(Timer timer)
        {
            timer.Resume();
        }

        [Button]
        public void ResumeAllTimers()
        {
            foreach (Timer timer in updateTimers)
            {
                timer.Resume();
            }

            foreach (Timer timer in fixedUpdateTimers)
            {
                timer.Resume();
            }
        }


        // Method to remove a specific timer
        public void RemoveTimer(Timer timer)
        {
            if (updateTimers.Contains(timer))
            {
                timer.Dispose();
                updateTimers.Remove(timer);
            }
            else if (fixedUpdateTimers.Contains(timer))
            {
                timer.Dispose();
                fixedUpdateTimers.Remove(timer);
            }
        }

        public void UpdateListTimer()
        {
            updateTimers.RemoveAll(timer => timer.count == 0 || timer.owner == null);
            fixedUpdateTimers.RemoveAll(timer => timer.count == 0 || timer.owner == null);
        }

        // Method to clear all timers
        [Button]
        public void ClearAllTimers()
        {
            foreach (Timer timer in updateTimers)
            {
                timer.Dispose();
            }

            updateTimers.Clear();

            foreach (Timer timer in fixedUpdateTimers)
            {
                timer.Dispose();
            }

            fixedUpdateTimers.Clear();
        }
    }
}