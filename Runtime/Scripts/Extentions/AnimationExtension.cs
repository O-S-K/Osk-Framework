using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OSK
{
    public static class AnimationExtension
    {
        public static void AddOrReplaceAnimationEvent(this AnimationClip clip, float time, string functionName)
        {
            AnimationEvent[] events = AnimationUtility.GetAnimationEvents(clip);
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
            AnimationUtility.SetAnimationEvents(clip, newEventsList.ToArray());
        }
    }
}