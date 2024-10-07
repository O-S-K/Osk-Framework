using System;
using UnityEngine;
using UnityEngine.UI;

namespace OSK
{
public class Painter : MonoBehaviour
{
    [SerializeField] private Color color = Color.black;
    [SerializeField] private int brushSize = 5;
    [SerializeField] private Texture2D cursor;
    [SerializeField] private Vector2 cursorHotspot;

    private Canvas canvas;
    private Texture2D texture;
    private Vector3 offset;
    private readonly Vector3[] corners = new Vector3[4];
    private Vector3 prev;
    private bool connect;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        var rectTransform = GetComponent<RectTransform>();
        var rect = rectTransform.rect;
        rectTransform.GetWorldCorners(corners);
        offset = corners[0];
        var image = GetComponent<RawImage>();
        texture = new Texture2D(Mathf.RoundToInt(rect.width), Mathf.RoundToInt(rect.height));
        image.texture = texture;
        Clear();
    }

    private void Clear()
    {
        for (var x = 0; x < texture.width; x++)
        {
            for (var y = 0; y < texture.height; y++)
            {
                texture.SetPixel(x, y, Color.clear);
            }
        }
        
        texture.Apply();
    }

    private void Plot(Vector2 pos, int radius, bool apply = true)
    {
        Plot(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), radius, apply);
    }
    
    private void Plot(int x, int y, int radius, bool apply = true)
    {
        for (var dx = -radius; dx < radius; dx++)
        {
            for (var dy = -radius; dy < radius; dy++)
            {
                var xx = x + dx;
                var yy = y + dy;
                var inside = xx > 0 && xx < texture.width - 1 && yy > 0 && yy < texture.height - 1;
                
                if (inside && new Vector2(dx, dy).magnitude <= radius)
                {
                    texture.SetPixel(xx, yy, color);   
                }
            }
        }

        if (apply)
        {
            
        }
        texture.Apply();
    }

    private void Line(Vector2 from, Vector2 to)
    {
        var distance = Vector3.Distance(from, to);
        var step = 1f / distance * brushSize;

        for (var i = 0f; i < 1f; i += step)
        {
            var p = Vector3.Lerp(from, to, i);
            Plot(p, brushSize, false);
        }
        texture.Apply();
    }

    private bool IsInside(Vector2 p)
    {
        return p.x > corners[0].x && p.y > corners[0].y && p.x < corners[2].x && p.y < corners[2].y;
    }

    private void OnDisable()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    private void Update()
    {
        var mp = Input.mousePosition;
        var holding = Input.GetMouseButton(0);
        var inside = IsInside(mp);
        
        var p = (mp - offset) / canvas.scaleFactor;

        if (cursor)
        {
            if (inside)
            {
                Cursor.SetCursor(cursor, cursorHotspot, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            }
        }
        
        if (holding && inside)
        {
            if (connect)
            {
                Line(p, prev);
            }
            else
            {
                Plot(p, brushSize);
            }
        }

        connect = holding;
        prev = p;
    }
}
}