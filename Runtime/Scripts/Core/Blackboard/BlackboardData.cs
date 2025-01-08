using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace OSK
{
    [CreateAssetMenu(fileName = "BlackBoardSO", menuName = "OSK/BlackBoardSO")]
    public class BlackboardData : ScriptableObject
    {
        [Serializable]
        public class BlackboardEntry : IComparable<BlackboardEntry>
        {
            public string key;
            public BlackboardValueType valueType;
            public int priority;
            public string category;
            public bool isReadOnly;
            public string description;
            public int intValue;    
            public float floatValue;
            public bool boolValue;
            public string stringValue;
            public Vector3 vector3Value;
            public Color colorValue;
            public UnityEngine.Object objectValue;

            public int CompareTo(BlackboardEntry other)
            {
                // Sort by priority first (higher priority first)
                int priorityComparison = other.priority.CompareTo(priority);
                if (priorityComparison != 0) return priorityComparison;
                
                // Then by category
                int categoryComparison = string.Compare(category, other.category, StringComparison.Ordinal);
                if (categoryComparison != 0) return categoryComparison;
                
                // Finally by key
                return string.Compare(key, other.key, StringComparison.Ordinal);
            }
            
            public BlackboardEntry Clone()
            {
                return (BlackboardEntry)this.MemberwiseClone();
            }
        }
        public enum BlackboardValueType
        {
            Int,
            Float,
            Bool,
            String,
            Vector3,
            Color,
            Object
        }
        public List<BlackboardEntry> entries = new List<BlackboardEntry>();

        private void OnEnable()
        {
            SortEntries();
        }

        public void SortEntries()
        {
            entries.Sort();
        }

        /// <summary>
        ///  Get entry by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public BlackboardEntry GetEntry(string key)
        {
            return entries.Find(e => e.key == key);
        }
        
        /*public BlackboardEntry GetKey(string key)
        {
            return entries.Find(e => e.key == key);
        }*/

        public bool HasKey(string key)
        {
            return entries.Exists(e => e.key == key);
        }

        public List<BlackboardEntry> GetEntriesByCategory(string category)
        {
            return entries.FindAll(e => e.category == category);
        }

        public List<BlackboardEntry> GetEntriesByPriority(int priority)
        {
            return entries.FindAll(e => e.priority == priority);
        }

        public void SetDefaultValues(Blackboard blackboard)
        {
            // Sort entries by priority before setting values
            SortEntries();
            
            foreach (var entry in entries)
            {
                if (!entry.isReadOnly || !blackboard.HasKey(entry.key))
                {
                    switch (entry.valueType)
                    {
                        case BlackboardValueType.Int:
                            blackboard.SetValue(entry.key, entry.intValue, entry.priority, entry.isReadOnly);
                            break;
                        case BlackboardValueType.Float:
                            blackboard.SetValue(entry.key, entry.floatValue, entry.priority, entry.isReadOnly);
                            break;
                        case BlackboardValueType.Bool:
                            blackboard.SetValue(entry.key, entry.boolValue, entry.priority, entry.isReadOnly);
                            break;
                        case BlackboardValueType.String:
                            blackboard.SetValue(entry.key, entry.stringValue, entry.priority, entry.isReadOnly);
                            break;
                        case BlackboardValueType.Vector3:
                            blackboard.SetValue(entry.key, entry.vector3Value, entry.priority, entry.isReadOnly);
                            break;
                        case BlackboardValueType.Color:
                            blackboard.SetValue(entry.key, entry.colorValue, entry.priority, entry.isReadOnly);
                            break;
                        case BlackboardValueType.Object:
                            blackboard.SetValue(entry.key, entry.objectValue, entry.priority, entry.isReadOnly);
                            break;
                    }
                }
            }
        }
    }
}
