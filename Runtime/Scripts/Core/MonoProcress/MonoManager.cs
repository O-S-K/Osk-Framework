using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

namespace OSK
{
    public class MonoManager : GameFrameworkComponent
    {
        private readonly List<Action> _toMainThreads = new();
        private volatile bool _isToMainThreadQueueEmpty = true;
        private List<Action> _localToMainThreads = new();

        internal event Action<bool> OnGamePause= null;
        internal event Action OnGameQuit = null;

        [ShowInInspector] private readonly List<IEntity> tickProcesses = new List<IEntity>(1024);
        [ShowInInspector] private readonly List<IEntity> fixedTickProcesses = new List<IEntity>(512);
        [ShowInInspector] private readonly List<IEntity> lateTickProcesses = new List<IEntity>(256);

        [ShowInInspector] public bool IsPause { get; private set; }
        [ShowInInspector] public float TimeScale { get; private set; }

        #region Set

        public override void OnInit()
        {
            IsPause = false;
            TimeScale = 1f;
        }

        public MonoManager SetTimeScale(float timeScale)
        {
            TimeScale = timeScale;
            Time.timeScale = TimeScale;
            return this;
        }

        public MonoManager SetPause(bool isPause, bool isSetTimeScale = true)
        {
            IsPause = isPause;
            return this;
        }

        #endregion

        #region Sub / UnSub For Update Procresses

        public void AddTickProcess(IEntity tick)
        {
            tickProcesses.Add(tick);
        }

        public void AddFixedTickProcess(IEntity fixedTick)
        {
            fixedTickProcesses.Add(fixedTick);
        }

        public void AddLateTickProcess(IEntity lateTick)
        {
            lateTickProcesses.Add(lateTick);
        }

        public void RemoveTickProcess(IEntity tick)
        {
            tickProcesses.Remove(tick);
        }

        public void RemoveFixedTickProcess(IEntity fixedTick)
        {
            fixedTickProcesses.Remove(fixedTick);
        }

        public void RemoveLateTickProcess(IEntity lateTick)
        {
            lateTickProcesses.Remove(lateTick);
        }

        public void RemoveAllTickProcess()
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

            for (var i = 0; i < _localToMainThreads.Count; i++)
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
            OnGamePause?.Invoke(hasFocus); // hasFocus = true when game is focus
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            OnGamePause?.Invoke(pauseStatus); // pauseStatus = true when game is pause
        }

        private void OnApplicationQuit()
        {
            OnGameQuit?.Invoke(); // Game is quit
        }

        #endregion

        #region Effective

        public Coroutine StartCoroutineImpl(IEnumerator routine)
        {
            if (routine != null)
            {
                return StartCoroutine(routine);
            }

            return null;
        }

        public Coroutine StartCoroutineImpl(string methodName, object value)
        {
            if (!string.IsNullOrEmpty(methodName))
            {
                return StartCoroutine(methodName, value);
            }

            return null;
        }

        public Coroutine StartCoroutineImpl(string methodName)
        {
            if (!string.IsNullOrEmpty(methodName))
            {
                return StartCoroutine(methodName);
            }

            return null;
        }

        public void StopCoroutineImpl(IEnumerator routine)
        {
            if (routine != null) StopCoroutine(routine);
        }

        public void StopCoroutineImpl(Coroutine routine)
        {
            if (routine != null) StopCoroutine(routine);
        }

        public void StopCoroutineImpl(string methodName)
        {
            if (!string.IsNullOrEmpty(methodName))
            {
                StopCoroutine(methodName);
            }
        }

        public void StopAllCoroutinesImpl()
        {
            StopAllCoroutines();
        }

        /// <summary>
        /// Schedules the specifies action to be run on the main thread (game thread).
        /// The action will be invoked upon the next Unity Update event.
        /// </summary>
        public void RunOnMainThreadImpl(Action action)
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
        public Action ToMainThreadImpl(Action action)
        {
            if (action == null) return delegate { };
            return () => RunOnMainThreadImpl(action);
        }

        /// <summary>
        /// Converts the specified action to one that runs on the main thread.
        /// The converted action will be invoked upon the next Unity Update event.
        /// </summary>
        public Action<T> ToMainThreadImpl<T>(Action<T> action)
        {
            if (action == null) return delegate { };
            return (arg) => RunOnMainThreadImpl(() => action(arg));
        }

        #endregion
    }
}