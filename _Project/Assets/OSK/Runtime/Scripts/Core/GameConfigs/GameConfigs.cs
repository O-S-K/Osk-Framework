using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigs : GameFrameworkComponent
{
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