using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBusExample : MonoBehaviour
{
    private int score = 0;
    private void Start()
    {
       World.EventBus.Subscribe<ScoreGameEventExample>(OnUpdateScore);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            World.EventBus.Publish(new ScoreGameEventExample(1));
        }
    }

    private void OnUpdateScore(ScoreGameEventExample data)
    {
        score += data.Score;
        Debug.Log("Score Updated! ->  " + score);
    }

    private void OnDestroy()
    {
        World.EventBus.Unsubscribe<ScoreGameEventExample>(OnUpdateScore);
    }
}
