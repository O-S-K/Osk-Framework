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


public class GameEvent
{
}

public class EventBus : GameFrameworkComponent
{
    private Dictionary<Type, List<Action<GameEvent>>> _subscribers = new Dictionary<Type, List<Action<GameEvent>>>();

    public void Subscribe<T>(Action<T> callback) where T : GameEvent
    {
        Type eventType = typeof(T);

        if (!_subscribers.ContainsKey(eventType))
        {
            _subscribers[eventType] = new List<Action<GameEvent>>();
        }

        _subscribers[eventType].Add(e => callback((T)e));
    }

    public void Unsubscribe<T>(Action<T> callback) where T : GameEvent
    {
        Type eventType = typeof(T);

        if (_subscribers.ContainsKey(eventType))
        {
            _subscribers[eventType].Remove(e => callback((T)e));
        }
    }

    public void Publish(GameEvent gameEvent)
    {
        Type eventType = gameEvent.GetType();

        if (_subscribers.ContainsKey(eventType))
        {
            foreach (var subscriber in _subscribers[eventType])
            {
                subscriber.Invoke(gameEvent);
            }
        }
    }
}