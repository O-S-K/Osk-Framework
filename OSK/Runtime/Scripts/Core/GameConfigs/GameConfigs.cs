using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
public class GameConfigs : GameFrameworkComponent
{
    public int targetFrameRate = 60;
    public int vSyncCount = 0;
    
    public string languageCodeDefault = "en";
    public Settings settingsDefault;
}

[System.Serializable]
public class Settings
{
    public bool isMusicOnDefault = true;
    public bool isSoundOnDefault = true;
    public bool isVibrationOnDefault = true;
    public bool isCheckInternetDefault = true;
    
}
}