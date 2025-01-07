using System;

namespace OSK
{
    public class BlackboardKey<T>
    {
        private readonly string key;
        private readonly Blackboard blackboard;

        public BlackboardKey(string key, Blackboard blackboard)
        {
            this.key = key;
            this.blackboard = blackboard;
        }

        #region Setter Getter

        public void SetValue(T value)
        {
            blackboard.SetValue(key, value);
        }

        public T GetValue()
        {
            return blackboard.GetValue<T>(key);
        }

        public bool TryGetValue(out T value)
        {
            return blackboard.TryGetValue(key, out value);
        }

        #endregion

        #region observer subs

        public void Subscribe(Action callback)
        {
            if (!blackboard.TryGetValue(key, out T value))
            {
                callback?.Invoke();
                blackboard.Subscribe(key, callback);
            }
        }

        public void Subscribe(Action<T> callback)
        {
            if (!blackboard.TryGetValue(key, out T value))
            {
                callback?.Invoke(value);
                blackboard.Subscribe(key, callback);
            }
        }

        public void Unsubscribe()
        {
            if (blackboard.TryGetValue(key, out T value))
            {
                blackboard.Unsubscribe(key);
            }
        }

        #endregion
    }
}