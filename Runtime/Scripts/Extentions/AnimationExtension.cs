using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

namespace OSK
{
    public static class AnimationExtension
    {
        #if UNITY_EDITOR
        public static void AddOrReplaceAnimationEvent(this AnimationClip clip, float time, string functionName)
        {
            AnimationEvent[] events = UnityEditor.AnimationUtility.GetAnimationEvents(clip);
            var newEventsList = new List<AnimationEvent>();
            foreach (var evt in events)
            {
                if (evt.functionName != functionName)
                {
                    newEventsList.Add(evt);
                }
            }

            AnimationEvent newEvent = new AnimationEvent
            {
                time = time,
                functionName = functionName
            };
            newEventsList.Add(newEvent);
            UnityEditor.AnimationUtility.SetAnimationEvents(clip, newEventsList.ToArray());
        }
        #endif
    }
}