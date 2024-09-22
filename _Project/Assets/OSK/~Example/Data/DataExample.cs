using System;
using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using OSK;
using UnityEngine;


public class DataExample : MonoBehaviour
{
    [Button("Check")]
    private void TestSave()
    {
        var test = new DataPlayerExample(123123, "TessadasdtName");
        World.Data.PlayerPrefs.SetObject("Test", test);
        Debug.Log("Test Saved");
        
        var test2 = World.Data.PlayerPrefs.GetObject<DataPlayerExample>("Test");
        Debug.Log("Test Loaded -> " + test2.Name + " " + test2.Score);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
             var player = new DataPlayerExample(100, "Player 1");
             World.Data.Json.Save<DataPlayerExample>(player, "Player");
             World.Data.File.SaveData<DataPlayerExample>("player", player);

            var p = new DataPlayerExample(100, "Player 1");
            World.Data.PlayerPrefs.SetObject("Player", p);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            // World.Data.Json.Load<DataPlayer>(player, "Player");
            // var p = World.Data.File.LoadData<DataPlayer>("player");
            var p = World.Data.PlayerPrefs.GetObject<DataPlayerExample>("Player");
            Debug.Log("Player Name -> " + p.Name);
            Debug.Log("Player Score -> " + p.Score);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            //World.Data.Json.Delete("Player");
            // World.Data.File.DeleteFile("player");
            World.Data.PlayerPrefs.Delete("Player");
        }
    }
}