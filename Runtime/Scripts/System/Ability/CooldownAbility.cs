using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
public class CooldownAbility : MonoBehaviour
{
    private Dictionary<Ability, float> cooldownTimers = new Dictionary<Ability, float>();

    public void StartCooldown(Ability ability)
    {
        if (!cooldownTimers.ContainsKey(ability))
        {
            cooldownTimers[ability] = ability.cooldownTime;
            CooldownRoutine(ability).Run();
        }
    }

    private IEnumerator CooldownRoutine(Ability ability)
    {
        while (cooldownTimers[ability] > 0)
        {
            cooldownTimers[ability] -= Time.deltaTime;
            yield return null;
        }
        cooldownTimers.Remove(ability);
        ability.isOnCooldown = false;
    }
}

}