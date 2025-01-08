using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace OSK
{
    public class EventSystemManager : MonoBehaviour
    {
        private void Awake()
        {
            EventSystem[] eventSystems = FindObjectsOfType<EventSystem>();

            if (eventSystems.Length > 1)
            {
                for (int i = 1; i < eventSystems.Length; i++)
                {
                    Destroy(eventSystems[i].gameObject);
                }
            }
            else if (eventSystems.Length == 0)
            {
                GameObject newEventSystem = new GameObject("EventSystem");
                newEventSystem.AddComponent<EventSystem>();
                newEventSystem.AddComponent<StandaloneInputModule>(); 
            }
        }
    }
}
