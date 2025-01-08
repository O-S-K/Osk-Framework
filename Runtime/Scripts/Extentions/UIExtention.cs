using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace OSK
{
    public static class UIExtention
    {
        public static void BindButton(this Button button, Action action)
        {
            if(button != null)
                button.onClick.AddListener(() => action());
        } 

        public static void BindEventTrigger(this EventTrigger trigger, EventTriggerType type,
            Action<BaseEventData> action)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = type;
            entry.callback.AddListener((data) => action(data));
            trigger.triggers.Add(entry);
        }
    }
}