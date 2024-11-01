using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class Observer : GameFrameworkComponent
    {
        public Dictionary<string, HashSet<CallBackObserver>> k_ObserverCallBack =  new Dictionary<string, HashSet<CallBackObserver>>();
        public delegate void CallBackObserver(object data);

        public override void OnInit() {}

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
            var keys = new List<string>(k_ObserverCallBack.Keys);
            foreach (string key in keys)
            {
                k_ObserverCallBack.Remove(key);
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
            if (!k_ObserverCallBack.ContainsKey(topicName))
            {
                k_ObserverCallBack.Add(topicName, new HashSet<CallBackObserver>());
            }
            return k_ObserverCallBack[topicName];
        }
    }
}