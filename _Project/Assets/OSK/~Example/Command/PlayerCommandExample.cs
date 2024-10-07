using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OSK;

public class PlayerCommandExample : MonoBehaviour, ICommand
{
    
    private Vector3 _previousPosition;
    private Vector3 _newPosition;
    
    public PlayerCommandExample(Vector3 newPosition)
    {
        _previousPosition = transform.position;
        _newPosition = newPosition;
    }

    public void MoveTo(Vector3 newPosition)
    {
        transform.position = newPosition;
        Debug.Log($"Player moved to {transform.position}");
    }

    public void Execute()
    {
        _previousPosition = transform.position; 
        MoveTo(_newPosition);
    }

    public void Undo()
    {
        MoveTo(_previousPosition);
    }
}
