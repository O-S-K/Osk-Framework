using System;

namespace OSK
{
    public class ProcedureNodeRemovedEventArgs : EventArgs
    {
        public Type NodeType { get; private set; }

        public static ProcedureNodeRemovedEventArgs Create(Type type)
        {
            return new ProcedureNodeRemovedEventArgs { NodeType = type };
        }

        public static void Release(ProcedureNodeRemovedEventArgs args)
        {
            args.NodeType = null;
        }
    }
}