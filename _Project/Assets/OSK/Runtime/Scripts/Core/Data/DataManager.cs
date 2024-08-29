using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;

public class DataManager : GameFrameworkComponent
{
      public static JsonSystem Json
      {
            get { return _json ?? (_json = new JsonSystem()); }
      }
      private static JsonSystem _json = new JsonSystem();
      
      public static FileSystem File
      {
            get { return _file ?? (_file = new FileSystem()); }
      }
      private static FileSystem _file = new FileSystem();
      
      public static PlayerPrefsSystem PlayerPrefs
      {
            get { return _playerPrefs ?? (_playerPrefs = new PlayerPrefsSystem()); }
      }
      private static PlayerPrefsSystem _playerPrefs = new PlayerPrefsSystem();
}
