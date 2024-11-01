using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace OSK
{
    public class CommandManager : GameFrameworkComponent
    {
        [SerializeReference]
        private Dictionary<string, Stack<ICommand>> k_CommandHistory = new Dictionary<string, Stack<ICommand>>();
        public override void OnInit() {}

        public void Create(string commandName, ICommand command)
        {
            if (!k_CommandHistory.ContainsKey(commandName))
            {
                k_CommandHistory[commandName] = new Stack<ICommand>();
            }

            command.Execute();
            k_CommandHistory[commandName].Push(command);
        }

        public void Undo(string commandName)
        {
            if (k_CommandHistory.ContainsKey(commandName) && k_CommandHistory[commandName].Count > 0)
            {
                ICommand command = k_CommandHistory[commandName].Pop();
                command.Undo();
            }
        }
    }
}