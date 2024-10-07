using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class Observer : GameFrameworkComponent
    {
        public Dictionary<string, HashSet<CallBackObserver>> dictObserver = new();
        public delegate void CallBackObserver(object data);

        public void Add(string topicName, CallBackObserver callbackObserver)
        {
            var listObserver = CreateListObserverForTopic(topicName);
            if (!listObserver.Contains(callbackObserver))
            {
                listObserver.Add(callbackObserver);
            }
        }

        public void Remove(string topicName, CallBackObserver callbackObserver)
        {
            var listObserver = CreateListObserverForTopic(topicName);
            if (listObserver.Contains(callbackObserver))
            {
                listObserver.Remove(callbackObserver);
            }
        }

        public void RemoveAllListeners()
        {
            var keys = new List<string>(dictObserver.Keys);
            foreach (string key in keys)
            {
                dictObserver.Remove(key);
            }
        }

        public void Notify<OData>(string topicName, OData Data)
        {
            var listObserver = CreateListObserverForTopic(topicName);
            foreach (var observer in listObserver)
            {
                observer(Data);
            }
        }

        public void Notify(string topicName)
        {
            var listObserver = CreateListObserverForTopic(topicName);
            foreach (var observer in listObserver)
            {
                observer(null);
            }
        }

        private HashSet<CallBackObserver> CreateListObserverForTopic(string topicName)
        {
            if (!dictObserver.ContainsKey(topicName))
            {
                dictObserver.Add(topicName, new HashSet<CallBackObserver>());
            }
            return dictObserver[topicName];
        }
    }
}