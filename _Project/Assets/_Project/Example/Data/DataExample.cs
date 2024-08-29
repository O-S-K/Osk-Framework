using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataExample : MonoBehaviour
{
    [System.Serializable]
    public class DataPlayer
    {
        public int Score { get; set; }
        public string Name { get; set; }
        
        public DataPlayer(int score, string name)
        {
            Score = score;
            Name = name;
        }
    }

    private DataPlayer player;
    
    private void Start()
    {
       
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            player = new DataPlayer(100, "Player 1");
            // World.Data.Json.Save<DataPlayer>(player, "Player");
            World.Data.File.SaveData<DataPlayer>("player", player);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            // World.Data.Json.Load<DataPlayer>(player, "Player");
            var p = World.Data.File.LoadData<DataPlayer>("player");
            Debug.Log("Player Name -> " + p.Name);
            Debug.Log("Player Score -> " + p.Score);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
           //World.Data.Json.Delete("Player");
           World.Data.File.DeleteFile("player");
        }
    }
}
