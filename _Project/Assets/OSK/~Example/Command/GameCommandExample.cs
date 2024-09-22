using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCommandExample : MonoBehaviour
{
    public PlayerCommandExample _player;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Vector3 newPosition = _player.transform.position + Vector3.one * Random.value;
            ICommand moveCommand = new MoveCommand(_player, newPosition);
            World.Command.Create("playermove", moveCommand);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            World.Command.Undo("playermove"); 
        }
    }
}
