using System;
using System.Collections.Generic;

namespace OSK
{
    public class ProcedureProcessor 
    {
        private ProcedureNode currentNode;
        private readonly Dictionary<Type, ProcedureNode> typeNodeDict = new();
        private Action<Type, Type> onProcedureNodeChange;
        private Action<Type> onProcedureNodeAdd;
        private Action<Type> onProcedureNodeRemove;

        // Event: Triggered when the current node changes
        public event Action<Type, Type> OnProcedureNodeChange
        {
            add => onProcedureNodeChange += value;
            remove => onProcedureNodeChange -= value;
        }

        // Event: Triggered when a new node is added
        public event Action<Type> OnProcedureNodeAdd
        {
            add => onProcedureNodeAdd += value;
            remove => onProcedureNodeAdd -= value;
        }

        // Event: Triggered when a node is removed
        public event Action<Type> OnProcedureNodeRemove
        {
            add => onProcedureNodeRemove += value;
            remove => onProcedureNodeRemove -= value;
        }

        // The current active ProcedureNode
        public ProcedureNode CurrentNode => currentNode;
 
        // Total number of nodes
        public int NodeCount => typeNodeDict.Count;
 

        /// <summary>
        /// Automatically finds and adds all ProcedureNode subclasses in the current AppDomain.
        /// </summary>
        public void FindAllProcedureNode()
        {
            Type baseType = typeof(ProcedureNode);
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(baseType) && !type.IsAbstract)
                    {
                        var nodeInstance = (ProcedureNode)Activator.CreateInstance(type);
                        AddNode(nodeInstance);
                    }
                }
            }
        }

        /// <summary>
        /// Adds a new ProcedureNode.
        /// </summary>
        public bool AddNode(ProcedureNode node)
        {
            var nodeType = node.GetType();
            if (!typeNodeDict.ContainsKey(nodeType))
            {
                typeNodeDict.Add(nodeType, node);
                node?.OnInit(this);
                onProcedureNodeAdd?.Invoke(nodeType);
                return true;
            }
            return false;
        }
        
        public bool AddNodes(params ProcedureNode[] nodes)
        {
            bool success = true;
            foreach (var node in nodes)
            {
                success &= AddNode(node);
            }
            return success;
        }

        /// <summary>
        /// Removes a ProcedureNode by type.
        /// </summary>
        public bool RemoveNode(Type nodeType)
        {
            if (typeNodeDict.TryGetValue(nodeType, out var node))
            {
                typeNodeDict.Remove(nodeType);
                node?.OnRemove(this);
                onProcedureNodeRemove?.Invoke(nodeType);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if a ProcedureNode of a specific type exists.
        /// </summary>
        public bool HasNode(Type nodeType) => typeNodeDict.ContainsKey(nodeType);

        /// <summary>
        /// Retrieves a ProcedureNode by type.
        /// </summary>
        public bool PeekNode(Type nodeType, out ProcedureNode node) => 
            typeNodeDict.TryGetValue(nodeType, out node);

        /// <summary>
        /// Updates the current ProcedureNode.
        /// </summary>
        public void OnUpdate()
        {
            currentNode?.OnUpdate(this);
        }
        
        /// <summary>
        /// FixedUpdates the current ProcedureNode.
        /// </summary>
        
        public void OnFixedUpdate()
        {
            currentNode?.OnFixedUpdate(this);
        }
        
        /// <summary>
        /// LateUpdates the current ProcedureNode.
        /// </summary>
        public void OnLateUpdate()
        {
            currentNode?.OnLateUpdate(this);
        }

        /// <summary>
        /// Switches the current ProcedureNode to a different type.
        /// </summary>
        public void ChangeNode(Type nodeType)
        {
            if (typeNodeDict.TryGetValue(nodeType, out var nextNode))
            {
                currentNode?.OnExit(this);
                var exitedType = currentNode?.GetType();

                currentNode = nextNode;
                currentNode?.OnEnter(this);

                var enteredType = currentNode?.GetType();
                onProcedureNodeChange?.Invoke(exitedType, enteredType);
            }
        }

        /// <summary>
        /// Clears all ProcedureNodes.
        /// </summary>
        public void ClearAllNode()
        {
            currentNode?.OnExit(this);
            foreach (var node in typeNodeDict.Values)
            {
                node?.OnRemove(this);
            }
            typeNodeDict.Clear();
        }

    }
}
