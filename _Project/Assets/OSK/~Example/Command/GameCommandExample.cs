using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;

public class GameCommandExample : MonoBehaviour
{
    public PlayerCommandExample _player;
    private string keyCommand = "GameCommandExample";
     
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Vector3 newPosition = _player.transform.position + Vector3.one * Random.value;
            ICommand moveCommand = new MoveCommand(_player, newPosition);
            Main.Command.Create(keyCommand, moveCommand);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Main.Command.Undo(keyCommand); 
        }
    }
}
