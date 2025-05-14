using System;

namespace OSK
{
    public interface IReferenceHolder
    {
        T GetRef<T>(string name, bool isLog = false) where T : UnityEngine.Object;

        void Foreach(Action<string, object> deal);
    }
}
