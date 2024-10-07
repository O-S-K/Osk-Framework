using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OSK;

public class ScoreGameEventExample : GameEvent
{
    public int Score { get; private set; }

    public ScoreGameEventExample(int score)
    {
        Score = score;
    }
} 