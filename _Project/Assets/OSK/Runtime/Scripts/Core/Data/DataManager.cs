using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;

public class DataManager : GameFrameworkComponent
{
    public JsonSystem Json
    {
        get { return _json ?? (_json = new JsonSystem()); }
    }

    private JsonSystem _json = new JsonSystem();

    public FileSystem File
    {
        get { return _file ?? (_file = new FileSystem()); }
    }

    private FileSystem _file = new FileSystem();

    public PlayerPrefsSystem PlayerPrefs
    {
        get { return _playerPrefs ?? (_playerPrefs = new PlayerPrefsSystem()); }
    }

    private PlayerPrefsSystem _playerPrefs = new PlayerPrefsSystem();
}