using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

public class AbilityManager : GameFrameworkComponent
{
    [ShowInInspector, ReadOnly] 
    private List<Ability> abilities = new List<Ability>();
 
    private void Update()
    {
        foreach (var ability in abilities)
        {
            if (ability.IsActive)
            {
                ability.UpdateAbility(gameObject);
            }
        }
    }

    public void ActivateAbility(int index)
    {
        if (index < abilities.Count && !abilities[index].isOnCooldown)
        {
            abilities[index].Activate(gameObject);
        }
    }
    
    
    public void ActivateAbility<AbilityType>() where AbilityType : Ability
    {
        foreach (var ability in abilities)
        {
            if (ability is AbilityType && !ability.isOnCooldown)
            {
                ability.Activate(gameObject);
                return;
            }
        }
    }

    
    public void ActiveAbility<AbilityType>() where AbilityType : Ability
    {
        foreach (var ability in abilities)
        {
            if (ability is AbilityType && !ability.isOnCooldown)
            {
                ability.Activate(gameObject);
                return;
            }
        }
    }
    
    public void DeactivateAbility(int index)
    {
        if (index < abilities.Count && abilities[index].IsActive)
        {
            abilities[index].Deactivate(gameObject);
        }
    }
    
    
    public void DeactivateAbility(Ability ability)
    {
        if (ability.IsActive)
        {
            ability.Deactivate(gameObject);
        }
    }
    
    public void DeactivateAbility<AbilityType>() where AbilityType : Ability
    {
        foreach (var ability in abilities)
        {
            if (ability is AbilityType && ability.IsActive)
            {
                ability.Deactivate(gameObject);
                return;
            }
        }
    }
    
    
    
    public void StartCooldown(Ability ability)
    {
        StartCoroutine(CooldownRoutine(ability));
    }
    
    private IEnumerator CooldownRoutine(Ability ability)
    {
        ability.StartCooldown();
        yield return new WaitForSeconds(ability.cooldownTime);
        ability.EndCooldown();
    }

    public void AddAbility(Ability newAbility)
    {
        if (abilities.Contains(newAbility))
        {
            return;
        }

        abilities.Add(newAbility);
    }

    public void RemoveAbility(Ability ability)
    {
        if (abilities.Contains(ability))
        {
            abilities.Remove(ability);
        }
    }
}