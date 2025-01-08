using System;

namespace OSK
{
    public class ProcedureNodeChangedEventArgs : EventArgs
    {
        public Type ExitedNodeType { get; private set; }
        public Type EnteredNodeType { get; private set; }

        public static ProcedureNodeChangedEventArgs Create(Type exited, Type entered)
        {
            return new ProcedureNodeChangedEventArgs { ExitedNodeType = exited, EnteredNodeType = entered };
        }

        public static void Release(ProcedureNodeChangedEventArgs args)
        {
            args.ExitedNodeType = null;
            args.EnteredNodeType = null;
        }
    }
}