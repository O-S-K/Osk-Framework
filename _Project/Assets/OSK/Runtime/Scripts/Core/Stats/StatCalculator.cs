using System.Collections.Generic;
using UnityEngine;

public class StatCalculator
{
    private Dictionary<string, Stats> stats =  new Dictionary<string, Stats>();

    public void AddStat(string name, float baseValue)
    {
        if (!stats.ContainsKey(name))
        {
            stats.Add(name, new Stats(name, baseValue));
            OSK.Logg.Log($"Stat {name} added with base value: {baseValue}");
        }
    }

    public void AddModifier(string statName, StatsModifier modifier)
    {
        if (stats.ContainsKey(statName))
        {
            stats[statName].AddModifier(modifier);
            OSK.Logg.Log($"{modifier.modifierType} modifier of {modifier.Value} applied to {statName}. New value: {GetFinalStatValue(statName)}");
        }
    }

    public void RemoveModifier(string statName, StatsModifier modifier)
    {
        if (stats.ContainsKey(statName))
        {
            stats[statName].RemoveModifier(modifier);
            OSK.Logg.Log($"{modifier.modifierType} modifier of {modifier.Value} removed from {statName}. New value: {GetFinalStatValue(statName)}");
        }
    }

    public float GetFinalStatValue(string statName)
    {
        if (stats.ContainsKey(statName))
        {
            return stats[statName].GetFinalValue();
        }
        return 0f;
    }
    
    public void UpdateStatValue(string statName, float value)
    {
        if (stats.ContainsKey(statName))
        {
            stats[statName].UpdateBaseValue(value);
            OSK.Logg.Log($"Stat {statName} updated to value: {value}");
        }
    }
}