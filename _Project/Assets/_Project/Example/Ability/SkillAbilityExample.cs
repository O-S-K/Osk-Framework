using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAbilityExample : MonoBehaviour
{
    public FireballAbilityExample ab;

    private void Start()
    {
        World.Ability.AddAbility(ab);
    }
     

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            World.Ability.ActivateAbility<FireballAbilityExample>();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            World.Ability.DeactivateAbility<FireballAbilityExample>();
        }

    }
}
