using System;
using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using OSK;
using UnityEngine;


public class SaveLoadDataExample : MonoBehaviour
{
    [Button("Check")]
    private void TestSave()
    {
        var test = new DataPlayerExample(123123, "TessadasdtName");
        Main.Save.PlayerPrefs.SetObject("Test", test);
        Debug.Log("Test Saved");
        
        var test2 = Main.Save.PlayerPrefs.GetObject<DataPlayerExample>("Test");
        Debug.Log("Test Loaded -> " + test2.Name + " " + test2.Score);
    }

    private void Start()
    {
        Main.Save.PlayerPrefs.SetBool("Test", true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
             var player = new DataPlayerExample(100, "Player 1");
             Main.Save.Json.Save<DataPlayerExample>(player, "Player");
             Main.Save.File.SaveData<DataPlayerExample>( "player", player);

            var p = new DataPlayerExample(100, "Player 1");
            Main.Save.PlayerPrefs.SetObject("Player", p);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            // World.Data.Json.Load<DataPlayer>(player, "Player");
            // var p = World.Data.File.LoadData<DataPlayer>("player");
            var p = Main.Save.PlayerPrefs.GetObject<DataPlayerExample>("Player");
            Debug.Log("Player Name -> " + p.Name);
            Debug.Log("Player Score -> " + p.Score);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            //World.Data.Json.Delete("Player");
            // World.Data.File.DeleteFile("player");
            Main.Save.PlayerPrefs.Delete("Player");
        }
    }
}