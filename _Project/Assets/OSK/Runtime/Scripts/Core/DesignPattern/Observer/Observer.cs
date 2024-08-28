
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class Observer: GameFrameworkComponent
    {
        public delegate void CallBackObserver(object data);
        public static Dictionary<string, HashSet<CallBackObserver>> dictObserver = new();

        public static void Add(string topicName, CallBackObserver callbackObserver)
        {
            HashSet<CallBackObserver> listObserver = CreateListObserverForTopic(topicName);
            listObserver.Add(callbackObserver);
        }

        public static void Remove(string topicName, CallBackObserver callbackObserver)
        {
            HashSet<CallBackObserver> listObserver = CreateListObserverForTopic(topicName);
            listObserver.Remove(callbackObserver);
        }

        public static void RemoveAllListeners()
        {
            List<string> keys = new List<string>(dictObserver.Keys);
            foreach (string key in keys)
            {
                dictObserver.Remove(key);
            }
        }

        public static void Notify<OData>(string topicName, OData Data)
        {
            HashSet<CallBackObserver> listObserver = CreateListObserverForTopic(topicName);
            foreach (CallBackObserver observer in listObserver)
            {
                observer(Data);
            }
        }

        public static void Notify(string topicName)
        {
            HashSet<CallBackObserver> listObserver = CreateListObserverForTopic(topicName);
            foreach (CallBackObserver observer in listObserver)
            {
                observer(null);
            }
        }

        protected static HashSet<CallBackObserver> CreateListObserverForTopic(string topicName)
        {
            if (!dictObserver.ContainsKey(topicName))
            {
                dictObserver.Add(topicName, new HashSet<CallBackObserver>());
            }
            return dictObserver[topicName];
        }
    }
}
