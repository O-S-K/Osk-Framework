using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OSK
{
    public class TimeManager : GameFrameworkComponent
    {
        // Lists to keep track of all active timers
        [ShowInInspector, ReadOnly] private List<Timer> _listUpdateTimers = new List<Timer>();
        [ShowInInspector, ReadOnly] private List<Timer> _listFixedUpdateTimers = new List<Timer>();

        public override void OnInit()
        {
        }

        // update every frame for a duration
        public Timer Create(MonoBehaviour owner)
        {
            return Create(owner.gameObject);
        }

        public Timer Create(GameObject owner)
        {
            var newTimer = new Timer();
            newTimer.Start(owner);

            if (newTimer.useFixedUpdate)
                _listFixedUpdateTimers.Add(newTimer);
            else
                _listUpdateTimers.Add(newTimer);

            return newTimer;
        }

        public DateTime GetRealTimeWorld()
        {
            WorldTimeAPI worldTimeAPI = new WorldTimeAPI();
            return worldTimeAPI.GetCurrentDateTime();
        }

        /// <summary>
        /// Create a timer that loops a certain number of times
        /// not use SetTimeOut, Add, Loops, UseFixedUpdate
        /// </summary>
        public Timer CreateLoops(MonoBehaviour owner, bool useFixedUpdate)
        {
            return CreateLoops(owner.gameObject, useFixedUpdate);
        }

        /// <summary>
        /// Create a timer that loops a certain number of times
        /// not use SetTimeOut, Add, Loops, UseFixedUpdate
        /// </summary>
        public Timer CreateLoops(GameObject owner, bool useFixedUpdate)
        {
            var newTimer = new Timer();
            newTimer.Start(owner).Loops(-1);

            if (useFixedUpdate)
                _listFixedUpdateTimers.Add(newTimer);
            else
                _listUpdateTimers.Add(newTimer);
            return newTimer;
        }

        public Timer CreateLoops(MonoBehaviour owner)
        {
            var newTimer = new Timer();
            newTimer.Start(owner.gameObject).Loops(-1);

            _listFixedUpdateTimers.Add(newTimer);
            _listUpdateTimers.Add(newTimer);
            return newTimer;
        }
 

        // Method to update all timers (called in Update)
        private void Update()
        {
            for (int i = _listUpdateTimers.Count - 1; i >= 0; i--)
            {
                var timer = _listUpdateTimers[i];
                timer.Update();

                // Remove the timer if it has been disposed
                if (timer == null || timer.Equals(null))
                {
                    _listUpdateTimers.RemoveAt(i);
                } 
            }
        }

        // Method to update all timers (called in FixedUpdate)
        private void FixedUpdate()
        {
            for (int i = _listFixedUpdateTimers.Count - 1; i >= 0; i--)
            {
                var timer = _listFixedUpdateTimers[i];
                timer.FixedUpdate();

                // Remove the timer if it has been disposed
                if (timer == null || timer.Equals(null))
                {
                    _listFixedUpdateTimers.RemoveAt(i);
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
            foreach (var timer in _listUpdateTimers)
            {
                timer.Pause();
            }

            foreach (var timer in _listFixedUpdateTimers)
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
            foreach (var timer in _listUpdateTimers) timer.Resume();
            foreach (var timer in _listFixedUpdateTimers) timer.Resume();
        }


        // Method to remove a specific timer
        public void RemoveTimer(Timer timer)
        {
            if (_listUpdateTimers.Contains(timer))
            {
                timer.Dispose();
                _listUpdateTimers.Remove(timer);
            }
            else if (_listFixedUpdateTimers.Contains(timer))
            {
                timer.Dispose();
                _listFixedUpdateTimers.Remove(timer);
            }
        }

        public void UpdateListTimer()
        {
            _listUpdateTimers.RemoveAll(timer => timer.count == 0 || timer.owner == null);
            _listFixedUpdateTimers.RemoveAll(timer => timer.count == 0 || timer.owner == null);
        }

        // Method to clear all timers
        [Button]
        public void ClearAllTimers()
        {
            foreach (var timer in _listUpdateTimers) timer.Dispose();
            _listUpdateTimers.Clear();

            foreach (var timer in _listFixedUpdateTimers) timer.Dispose();
            _listFixedUpdateTimers.Clear();
        }
    }
}