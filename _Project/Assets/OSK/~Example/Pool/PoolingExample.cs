using System.Collections;
using System.Collections.Generic;
using OSK.Utils;
using UnityEngine;

public class PoolingExample : MonoBehaviour
{
    [SerializeField] private BulletPoolExample bulletPrefab;
    
    [SerializeField] private GameObject spherePrefab;

    private void Start()
    {
        Main.Pool.Preload("Bullet", bulletPrefab, 10);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var bullet = Main.Pool.Spawn("Sphere", spherePrefab);
            bullet.transform.position =  new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
            
            this.DoDelay(1, () =>
            {
                Main.Pool.Despawn(bullet);
            });
        }
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            var bullet = Main.Pool.Get("Sphere", spherePrefab);
            bullet.transform.position =  new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
               
            this.DoDelay(1, () =>
            {
                Main.Pool.Despawn(bullet);
            });
        }
        
        // if (Input.GetKeyDown(KeyCode.G))
        // {
        //     var bullet = Main.Pool.Spawn("Bullet", bulletPrefab);
        //     bullet.transform.position =  new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
        // }
        //
        // if (Input.GetKey(KeyCode.H))
        // {
        //     var bullet = Main.Pool.Spawn("Bullet2", bulletPrefab);
        //     bullet.transform.position =  new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
        // }
        // if (Input.GetKeyDown(KeyCode.F))
        // {
        //     Main.Pool.DespawnAllInGroup("Bullet");
        // }
    }
}
