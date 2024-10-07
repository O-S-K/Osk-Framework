using System;
using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;
using OSK;


public class DataHanderExample : MonoBehaviour
{
    [SerializeField] private SerializeFieldDictionary<string, int> _dataStore1 = new(); 
    [SerializeField] private SerializeFieldDictionary<string, GameObject> _dataStore2 = new(); 


    [Button]
    private void CreateData()
    {
        var player0 = new DataPlayerExample(10, "Player 0");
        var player1 = new DataPlayerExample(20, "Player 1");
        var player2 = new DataPlayerExample(60, "Player 2");
        var player3 = new DataPlayerExample(100, "Player 3");

        Main.Data.Add(player0);
        Main.Data.Add(new List<DataPlayerExample>()
        {
            player1,
            player2,
            player3
        });
        
        Logg.Log("Data Created");
        var lp = Main.Data.GetAll<DataPlayerExample>();
        lp.ForEach(p =>
        {
            Logg.LogFormat("{0, -10} {1, -10}", p.Name, p.Score);
        });
    }
    
    [Button]
    private void ShowFormatTable()
    {
        Logg.LogFormat("{0, -10} {1, -10}", "Name", "Score");
        var players = Main.Data.GetAll<DataPlayerExample>();
        players.ForEach(p =>
        {
            Logg.LogFormat("{0, -10} {1, -10}", p.Name, p.Score);
        });
    }
    
    [Button]
    public void LoadAllData()
    {
        Logg.Log("Load All Data");
        var players = Main.Data.GetAll<DataPlayerExample>();
        players.ForEach(p =>
        {
             Logg.LogFormat("{0, -10} {1, -10}", p.Name, p.Score);
        });
    }

    [Button]
    private void LoadFirstData()
    {
        Logg.Log("Load First Data");
        var player = Main.Data.Get<DataPlayerExample>();
        Logg.LogFormat("{0, -10} {1, -10}", player.Name, player.Score);
    }

    [Button]
    private void QueryData()
    {
        Logg.Log("Query Data");
        var player = Main.Data.QueriesAll<DataPlayerExample>(p => p.Score > 50);
        player.ForEach(p =>
        {
            Logg.LogFormat("{0, -10} {1, -10}", p.Name, p.Score);
        }); 
        
        var p1 = Main.Data.Query<DataPlayerExample>(p => p.Name == "Player 1");
        Logg.LogFormat("{0, -10} {1, -10}", p1.Name, p1.Score);
    }
    
     
    [Button]
    private void RandomData()
    {
        Logg.Log("Random Data");
        var player = Main.Data.QueriesAll<DataPlayerExample>(p => p.Name == "Player 1");
        player.ForEach(p =>
        {
            p.Score = UnityEngine.Random.Range(1, 1000);
            Logg.LogFormat("{0, -10} {1, -10}", p.Name, p.Score);
        });
    }
}