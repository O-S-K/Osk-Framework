using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public abstract class ProcedureNode 
    {
        /// <summary>
        /// Called when the node is initialized.
        /// </summary>
        public abstract void OnInit(ProcedureProcessor processor);

        /// <summary>
        /// Called when the node becomes active.
        /// </summary>
        public abstract void OnEnter(ProcedureProcessor processor);

        /// <summary>
        /// Called continuously while the node is active.
        /// </summary>
        public abstract void OnUpdate(ProcedureProcessor processor);
        
        /// <summary>
        /// called continuously at a fixed interval.
        ///  </summary>
        public abstract void OnFixedUpdate(ProcedureProcessor processor);
        
        /// <summary>
        /// called continuously at a fixed interval.
        ///  </summary>
        public abstract void OnLateUpdate(ProcedureProcessor processor);

        /// <summary>
        /// Called when the node is exited.
        /// </summary>
        public abstract void OnExit(ProcedureProcessor processor);

        /// <summary>
        /// Called when the node is removed from the processor.
        /// </summary>
        public abstract void OnRemove(ProcedureProcessor processor);

        /// <summary>
        /// Switch to another ProcedureNode.
        /// </summary>
        protected void ChangeState<T>(ProcedureProcessor processor) where T : ProcedureNode
        {
            processor.ChangeNode(typeof(T));
        }

        /// <summary>
        /// Switch to another ProcedureNode by type.
        /// </summary>
        protected void ChangeState(ProcedureProcessor processor, Type stateType)
        {
            processor.ChangeNode(stateType);
        }
    }
}
