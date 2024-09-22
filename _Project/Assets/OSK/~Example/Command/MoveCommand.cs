using UnityEngine;

public class MoveCommand : ICommand
{
    private PlayerCommandExample _player;
    private Vector3 _previousPosition;
    private Vector3 _newPosition;

    public MoveCommand(PlayerCommandExample player, Vector3 newPosition)
    {
        _player = player;
        _previousPosition = player.transform.position;
        _newPosition = newPosition;
    }

    public void Execute()
    {
        _previousPosition = _player.transform.position;  
        _player.MoveTo(_newPosition); 
    }

    public void Undo()
    {
        _player.MoveTo(_previousPosition);  
    }
}