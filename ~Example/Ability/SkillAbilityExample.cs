using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OSK;

public class SkillAbilityExample : MonoBehaviour
{
    public FireballAbilityExample ab;

    private void Start()
    {
        //Main.Ability.AddAbility(ab);
    }
     

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Main.Ability.ActivateAbility<FireballAbilityExample>();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //Main.Ability.DeactivateAbility<FireballAbilityExample>();
        }

    }
}
