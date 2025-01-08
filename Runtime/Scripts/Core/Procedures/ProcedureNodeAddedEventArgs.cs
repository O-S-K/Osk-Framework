using System;

namespace OSK
{
    public class ProcedureNodeAddedEventArgs : EventArgs
    {
        public Type NodeType { get; private set; }

        public static ProcedureNodeAddedEventArgs Create(Type type)
        {
            return new ProcedureNodeAddedEventArgs { NodeType = type };
        }

        public static void Release(ProcedureNodeAddedEventArgs args)
        {
            args.NodeType = null;
        }
    }
}