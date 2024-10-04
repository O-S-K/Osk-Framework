using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;

public class PlayerSkillExample : EComponent
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Main.Ability.ActivateAbility<FireballAbilityExample>();
        }
    }
}
