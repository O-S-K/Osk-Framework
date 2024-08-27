using System.Collections;
using System.Collections.Generic;
using OSK.Sound;
using UnityEngine;

public partial class World : MonoBehaviour
{
    public static GameConfigs Configs { get; private set; }
    public static UIManager UI { get; private set; }
    
    public static SoundManager Sound { get; private set; }

    protected void Init()
    {
        Configs = transform.GetComponentInChildren<GameConfigs>();
        UI = transform.GetComponentInChildren<UIManager>();
        Sound = transform.GetComponentInChildren<SoundManager>();
    }
}