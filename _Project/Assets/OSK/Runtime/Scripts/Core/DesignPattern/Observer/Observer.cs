
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class Observer: GameFrameworkComponent
    {
        public delegate void CallBackObserver(object data);
        public  Dictionary<string, HashSet<CallBackObserver>> dictObserver = new();

        public  void Add(string topicName, CallBackObserver callbackObserver)
        {
            HashSet<CallBackObserver> listObserver = CreateListObserverForTopic(topicName);
            listObserver.Add(callbackObserver);
        }

        public  void Remove(string topicName, CallBackObserver callbackObserver)
        {
            HashSet<CallBackObserver> listObserver = CreateListObserverForTopic(topicName);
            listObserver.Remove(callbackObserver);
        }

        public  void RemoveAllListeners()
        {
            List<string> keys = new List<string>(dictObserver.Keys);
            foreach (string key in keys)
            {
                dictObserver.Remove(key);
            }
        }

        public  void Notify<OData>(string topicName, OData Data)
        {
            HashSet<CallBackObserver> listObserver = CreateListObserverForTopic(topicName);
            foreach (CallBackObserver observer in listObserver)
            {
                observer(Data);
            }
        }

        public  void Notify(string topicName)
        {
            HashSet<CallBackObserver> listObserver = CreateListObserverForTopic(topicName);
            foreach (CallBackObserver observer in listObserver)
            {
                observer(null);
            }
        }

        protected  HashSet<CallBackObserver> CreateListObserverForTopic(string topicName)
        {
            if (!dictObserver.ContainsKey(topicName))
            {
                dictObserver.Add(topicName, new HashSet<CallBackObserver>());
            }
            return dictObserver[topicName];
        }
    }
}
