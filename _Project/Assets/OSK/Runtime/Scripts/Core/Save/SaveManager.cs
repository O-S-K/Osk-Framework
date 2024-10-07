using System;
using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;

namespace OSK
{
public class SaveManager : GameFrameworkComponent
{
    public JsonSystem Json => _json ??= new JsonSystem();
    private JsonSystem _json = new JsonSystem();

    public FileSystem File => _file ??= new FileSystem();
    private FileSystem _file = new FileSystem();

    public PlayerPrefsSystem PlayerPrefs => _playerPrefs ??= new PlayerPrefsSystem();
    private PlayerPrefsSystem _playerPrefs = new PlayerPrefsSystem();
    
    private ScriptableObjectManager _scriptableObjectManager;
    public ScriptableObjectManager SOData => _scriptableObjectManager ??= new ScriptableObjectManager();
}
}