using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollMaterialUI : MonoBehaviour
{
    // Speed of scrolling in the X (horizontal) and Y (vertical) directions
    public float scrollSpeedX = 0.1f;
    public float scrollSpeedY = 0.1f;

    // Reference to the Renderer of the object
    private Image rend;

    void Start()
    {
        // Get the Renderer component attached to this GameObject
        rend = GetComponent<Image>();
    }

    void Update()
    {
        // Calculate the new texture offset
        float offsetX = Time.time * scrollSpeedX;
        float offsetY = Time.time * scrollSpeedY;

        // Apply the offset to the material's main texture
        rend.material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));
    }
}
