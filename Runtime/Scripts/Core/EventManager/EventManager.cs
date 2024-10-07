using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    // example usage:
    // EventManager.Subscribe("ScoreChanged", OnScoreChanged);
    // EventManager.Notify("ScoreChanged", this, new ScoreChangedEvent(100));
    // EventManager.Unsubscribe("ScoreChanged", OnScoreChanged);
    
    
    public interface IGameEvent { }
    public static class EventManager
    {
        private static Dictionary<string, Action<object, IGameEvent>> _subscribers = new ();

        public static void Subscribe(string eventID, Action<object, IGameEvent> callback)
        {
            if (!_subscribers.TryAdd(eventID, callback))
                _subscribers[eventID] += callback;
        }

        public static void Unsubscribe(string eventID, Action<object, IGameEvent> callback)
        {
            if (_subscribers.ContainsKey(eventID))
                _subscribers[eventID] -= callback;
        }

        public static void Notify(string eventID, object sender, IGameEvent gameEvent)
        {
            if (_subscribers.TryGetValue(eventID, out var selectedCallback))
            {
                // let this throw an exception so that it is properly handled during the development stage
                Debug.Assert(selectedCallback != null, "There are no subscribed events for this " + eventID);
                selectedCallback(sender, gameEvent);
            }
        }
    }
}
