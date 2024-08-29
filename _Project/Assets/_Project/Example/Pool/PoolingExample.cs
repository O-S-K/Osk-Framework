using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingExample : MonoBehaviour
{
    [SerializeField] private BulletPoolExample bulletPrefab;
    private void Start()
    {
        World.Pool.WarmPool(bulletPrefab, 10);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            var bullet = World.Pool.Spawn(bulletPrefab);
            bullet.transform.position =  new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            World.Pool.DespawnAll();
        }
    }
}
