using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Stats
{
    public string Name { get; private set; }
    public float BaseValue { get; private set; }
    public float FinalValue { get; private set; }

    private List<StatsModifier> modifiers;

    public Stats(string name, float baseValue)
    {
        Name = name;
        BaseValue = baseValue;
        modifiers = new List<StatsModifier>();
    }

    public void AddModifier(StatsModifier modifier)
    {
        modifiers.Add(modifier);
    }

    public void RemoveModifier(StatsModifier modifier)
    {
        modifiers.Remove(modifier);
    }

    public float GetFinalValue()
    {
        // Tính toán tổng số modifier dạng flat và phần trăm
        float flatModifierTotal = modifiers.Where(m => m.modifierType == ModifierType.Flat).Sum(m => m.Value);
        float percentModifierTotal = modifiers.Where(m => m.modifierType == ModifierType.Percent).Sum(m => m.Value);

        FinalValue = (BaseValue + flatModifierTotal) * (1 + percentModifierTotal);
        return FinalValue;
    }

    public void UpdateBaseValue(float value)
    {
        BaseValue = value;
    }
}