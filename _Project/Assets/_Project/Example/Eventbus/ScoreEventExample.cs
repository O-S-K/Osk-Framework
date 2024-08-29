using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreEventExample : GameEvent
{
    public int Score { get; private set; }

    public ScoreEventExample(int score)
    {
        Score = score;
    }
} 