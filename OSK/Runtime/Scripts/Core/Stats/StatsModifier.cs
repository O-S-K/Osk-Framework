using UnityEngine;

namespace OSK
{
public class StatsModifier
{
    public ModifierType modifierType;
    public float Value;

    public StatsModifier(ModifierType modifierType, float value)
    {
        this.modifierType = modifierType;
        this.Value = value;
    }
    
}

}