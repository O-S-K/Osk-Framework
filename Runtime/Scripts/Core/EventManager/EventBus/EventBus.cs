using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
//          ScoreChangedEvent : GameEvent
start:      EventBus.Subscribe<ScoreEventExample>(OnUpdateScore);
            OnUpdateScore(ScoreEventExample data)
destroy:    EventBus.Unsubscribe<ScoreEventExample>(OnUpdateScore);
//          EventBus.Publish(new ScoreEventExample(newScore));
 */


namespace OSK
{
    public class GameEvent { }

    public class EventBus : GameFrameworkComponent
    {
        private Dictionary<Type, List<Action<GameEvent>>> k_Subscribers = new Dictionary<Type, List<Action<GameEvent>>>();

        public override void OnInit()
        { 
        }
        
        public void Subscribe<T>(Action<T> callback) where T : GameEvent
        {
            Type eventType = typeof(T);

            if (!k_Subscribers.ContainsKey(eventType))
            {
                k_Subscribers[eventType] = new List<Action<GameEvent>>();
            }

            k_Subscribers[eventType].Add(e => callback((T)e));
        }

        public void Unsubscribe<T>(Action<T> callback) where T : GameEvent
        {
            Type eventType = typeof(T);

            if (k_Subscribers.ContainsKey(eventType))
            {
                // check null
                k_Subscribers[eventType].Remove(e =>
                {
                    if (e != null)
                        callback((T)e);
                });
            }
        }

        public void Publish(GameEvent gameGameEvent)
        {
            Type eventType = gameGameEvent.GetType();

            if (k_Subscribers.ContainsKey(eventType))
            {
                foreach (var subscriber in k_Subscribers[eventType])
                {
                    subscriber?.Invoke(gameGameEvent);
                }
            }
        }
    }
}