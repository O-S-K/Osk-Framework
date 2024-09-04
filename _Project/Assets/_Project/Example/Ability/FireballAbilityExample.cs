using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FireballAbilityExample/Fireball")]
public class FireballAbilityExample : Ability, IComponent
{
    public GameObject fireballPrefab;
    public float fireballSpeed;
    public float damage;
    private GameObject fireball;
    
    public override void Activate(GameObject user)
    {
        base.Activate(user);
        // Instantiate and launch the fireball
        fireball = Instantiate(fireballPrefab, user.transform.position, Quaternion.identity);
        //fireball.GetComponent<Rigidbody>().velocity = user.transform.forward * fireballSpeed;
        Destroy(fireball, 5);
    }
    
    public override void UpdateAbility(GameObject user)
    {
    }

    public override void Deactivate(GameObject user)
    {
        if(fireball != null) 
            Destroy(fireball);
        isActivated = false;
        base.Deactivate(user);
    }
}