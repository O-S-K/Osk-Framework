using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class CommandManager : GameFrameworkComponent
    {
        [SerializeReference] private Dictionary<string, Stack<ICommand>> _commandHistory = new();

        public void Create(string commandName, ICommand command)
        {
            if (!_commandHistory.ContainsKey(commandName))
            {
                _commandHistory[commandName] = new Stack<ICommand>();
            }

            command.Execute();
            _commandHistory[commandName].Push(command);
        }

        public void Undo(string commandName)
        {
            if (_commandHistory.ContainsKey(commandName) && _commandHistory[commandName].Count > 0)
            {
                ICommand command = _commandHistory[commandName].Pop();
                command.Undo();
            }
        }
    }
}