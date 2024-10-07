using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
public class Rotator : MonoBehaviour
{
    public float speed = 1f;
    private float angle = 0f;


    private void Update()
    {
        angle += speed * Time.deltaTime * 60f;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void ChangeSpeed(float s)
    {
        speed = s;
    }
}
}