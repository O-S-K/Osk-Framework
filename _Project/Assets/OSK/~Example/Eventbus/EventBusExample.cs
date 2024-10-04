using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBusExample : MonoBehaviour
{
    private int score = 0;
    private void Start()
    {
       Main.EventBus.Subscribe<ScoreGameEventExample>(OnUpdateScore);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Main.EventBus.Publish(new ScoreGameEventExample(1));
        }
    }

    private void OnUpdateScore(ScoreGameEventExample data)
    {
        score += data.Score;
        Debug.Log("Score Updated! ->  " + score);
    }

    private void OnDestroy()
    {
        Main.EventBus.Unsubscribe<ScoreGameEventExample>(OnUpdateScore);
    }
}
