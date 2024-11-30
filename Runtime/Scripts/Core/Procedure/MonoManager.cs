using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace OSK
{
    public class MonoManager : GameFrameworkComponent
    {
        private readonly List<Action> _toMainThreads = new();
        private volatile bool _isToMainThreadQueueEmpty = true;
        private List<Action> _localToMainThreads = new();

        internal event Action<bool> OnGamePause;
        internal event Action OnGameQuit;
        internal event Action<bool> OnGameFocus;

        readonly List<IEntity> tickProcesses = new List<IEntity>(1024);
        readonly List<IEntity> fixedTickProcesses = new List<IEntity>(512);
        readonly List<IEntity> lateTickProcesses = new List<IEntity>(256);

        public bool IsPause;

        public override void OnInit()
        {
        }

        #region Sub / UnSub For Update Procresses

        internal void AddTickProcess(IEntity tick)
        {
            tickProcesses.Add(tick);
        }

        internal void AddFixedTickProcess(IEntity fixedTick)
        {
            fixedTickProcesses.Add(fixedTick);
        }

        internal void AddLateTickProcess(IEntity lateTick)
        {
            lateTickProcesses.Add(lateTick);
        }

        internal void RemoveTickProcess(IEntity tick)
        {
            tickProcesses.Remove(tick);
        }

        internal void RemoveFixedTickProcess(IEntity fixedTick)
        {
            fixedTickProcesses.Remove(fixedTick);
        }

        internal void RemoveLateTickProcess(IEntity lateTick)
        {
            lateTickProcesses.Remove(lateTick);
        }
        
        internal void RemoveAllTickProcess()
        {
            tickProcesses.Clear();
            fixedTickProcesses.Clear();
            lateTickProcesses.Clear();
        }

        #endregion

        #region Update Handle

        private void Update()
        {
            if (IsPause) return;

            for (int i = 0; i < tickProcesses.Count; i++)
            {
                tickProcesses[i]?.Tick();
            }

            if (_isToMainThreadQueueEmpty) return;
            _localToMainThreads.Clear();
            lock (_toMainThreads)
            {
                _localToMainThreads.AddRange(_toMainThreads);
                _toMainThreads.Clear();
                _isToMainThreadQueueEmpty = true;
            }

            for (int i = 0; i < _localToMainThreads.Count; i++)
            {
                _localToMainThreads[i].Invoke();
            }
        }

        private void FixedUpdate()
        {
            if (IsPause) return;

            for (int i = 0; i < fixedTickProcesses.Count; i++)
            {
                fixedTickProcesses[i]?.FixedTick();
            }
        }

        private void LateUpdate()
        {
            if (IsPause) return;

            for (int i = 0; i < lateTickProcesses.Count; i++)
            {
                lateTickProcesses[i]?.LateTick();
            }
        }

        #endregion

        #region App Handle

        private void OnApplicationFocus(bool hasFocus)
        {
            OnGamePause?.Invoke(hasFocus);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            OnGamePause?.Invoke(pauseStatus);
            if (pauseStatus)
            {
                // Save data
            }
        }

        private void OnApplicationQuit()
        {
            OnGameQuit?.Invoke();
            // Save data
        }

        #endregion

        #region Effective

        internal Coroutine StartCoroutineImpl(IEnumerator routine)
        {
            if (routine != null)
            {
                return StartCoroutine(routine);
            }

            return null;
        }

        internal Coroutine StartCoroutineImpl(string methodName, object value)
        {
            if (!string.IsNullOrEmpty(methodName))
            {
                return StartCoroutine(methodName, value);
            }

            return null;
        }

        internal Coroutine StartCoroutineImpl(string methodName)
        {
            if (!string.IsNullOrEmpty(methodName))
            {
                return StartCoroutine(methodName);
            }

            return null;
        }

        internal void StopCoroutineImpl(IEnumerator routine)
        {
            if (routine != null) StopCoroutine(routine);
        }

        internal void StopCoroutineImpl(Coroutine routine)
        {
            if (routine != null) StopCoroutine(routine);
        }

        internal void StopCoroutineImpl(string methodName)
        {
            if (!string.IsNullOrEmpty(methodName))
            {
                StopCoroutine(methodName);
            }
        }

        internal void StopAllCoroutinesImpl()
        {
            StopAllCoroutines();
        }

        /// <summary>
        /// Schedules the specifies action to be run on the main thread (game thread).
        /// The action will be invoked upon the next Unity Update event.
        /// </summary>
        internal void RunOnMainThreadImpl(Action action)
        {
            lock (_toMainThreads)
            {
                _toMainThreads.Add(action);
                _isToMainThreadQueueEmpty = false;
            }
        }

        /// <summary>
        /// Converts the specified action to one that runs on the main thread.
        /// The converted action will be invoked upon the next Unity Update event.
        /// </summary>
        internal Action ToMainThreadImpl(Action action)
        {
            if (action == null) return delegate { };
            return () => RunOnMainThreadImpl(action);
        }

        /// <summary>
        /// Converts the specified action to one that runs on the main thread.
        /// The converted action will be invoked upon the next Unity Update event.
        /// </summary>
        internal Action<T> ToMainThreadImpl<T>(Action<T> action)
        {
            if (action == null) return delegate { };
            return (arg) => RunOnMainThreadImpl(() => action(arg));
        }

        /// <summary>
        /// Converts the specified action to one that runs on the main thread.
        /// The converted action will be invoked upon the next Unity Update event.
        /// </summary>
        internal Action<T1, T2> ToMainThreadImpl<T1, T2>(Action<T1, T2> action)
        {
            if (action == null) return delegate { };
            return (arg1, arg2) => RunOnMainThreadImpl(() => action(arg1, arg2));
        }

        /// <summary>
        /// Converts the specified action to one that runs on the main thread.
        /// The converted action will be invoked upon the next Unity Update event.
        /// </summary>
        internal Action<T1, T2, T3> ToMainThreadImpl<T1, T2, T3>(Action<T1, T2, T3> action)
        {
            if (action == null) return delegate { };
            return (arg1, arg2, arg3) => RunOnMainThreadImpl(() => action(arg1, arg2, arg3));
        }

        #endregion
    }
}