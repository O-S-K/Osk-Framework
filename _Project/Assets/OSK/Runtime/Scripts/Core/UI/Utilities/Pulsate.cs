using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulsate : MonoBehaviour
{
    public float amount = 0.1f;
    public float speed = 1f;
    public bool oneSided = false;

    public float delay;
    private float currentDelay;

    public Vector3 ratio = Vector3.one;
    private Vector3 originalScale;

    private OSK.Appearer appearer;
    private ButtonEffect buttonEffect;

    private void OnEnable()
    {
        appearer = GetComponent<OSK.Appearer>();
        buttonEffect = GetComponent<ButtonEffect>();

        if (appearer != null)
        {
            currentDelay = delay + appearer.duration + appearer.appearAfter;
            originalScale = Vector3.one;
        }
        else
        {
            currentDelay = delay;
            originalScale = transform.localScale;
        }
    }

    private void Update()
    {
        if (buttonEffect != null && buttonEffect.IsPressed)
        {
            return;
        }

        if (currentDelay > 0)
        {
            currentDelay -= Time.deltaTime;
        }
        else
        {
            float amt = Mathf.Sin(Time.time * speed);
            amt = oneSided ? Mathf.Abs(amt) : amt;
            transform.localScale = originalScale + ratio * (amount * amt);
        }
    }
}