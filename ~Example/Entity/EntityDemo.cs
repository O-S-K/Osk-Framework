using System;
using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;

public class EntityDemo : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        Main.Entity.Create<HeroExample>("Player");
        Main.Entity.Create<HeroExample>("Player1");
        Main.Entity.Create<HeroExample>("Player2");
        Main.Entity.Create<HeroExample>("Player3");
        Main.Entity.Create<HeroExample>("Player4");

        yield return null;
        HeroExample player = Main.Entity.Get<HeroExample>("Player");
        yield return new WaitForSeconds(1);
        player.Attack();

        yield return new WaitForSeconds(3);
        Main.Entity.Remove(player.GetComponent<Entity>());
    }
}
