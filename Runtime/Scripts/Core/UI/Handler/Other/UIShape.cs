using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(CanvasRenderer))]
public class UIShape : Graphic
{
    [Header("Shape Settings")]
    [SerializeField, Range(3, 100)] private int segments = 6;  
    [SerializeField, Range(0, 1)] private float fillPercent = 1f;  
    [SerializeField] private bool isClockwise = true;
    [SerializeField, Range(0, 100)] private float thickness = 5f;  
    [SerializeField, Range(0, 360)] private float rotationAngle = 0f; 
 
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        DrawCircle(vh);
    }

    private void DrawCircle(VertexHelper vh)
    {
        vh.Clear();

        float angleStep = (2 * Mathf.PI) / segments;
        float outerRadius = rectTransform.rect.width / 2;
        if(segments <= 3)
        {
            thickness = Mathf.Min(thickness, 50);
        }
        float innerRadius = outerRadius - thickness;
        Vector2 center = Vector2.zero;

        List<UIVertex> vertices = new List<UIVertex>();
        List<int> indices = new List<int>();

        if(isClockwise)
        {
            angleStep = -angleStep;
        }
        int segmentCount = Mathf.CeilToInt(segments * fillPercent);

        // Tạo các điểm của viền ngoài và viền trong
        for (int i = 0; i <= segmentCount; i++)
        {
            float angle = i * angleStep + Mathf.Deg2Rad * rotationAngle;
            Vector2 outerPoint = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * outerRadius;
            Vector2 innerPoint = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * innerRadius;

            vertices.Add(CreateVertex(outerPoint, color)); // Viền ngoài
            vertices.Add(CreateVertex(innerPoint, color)); // Viền trong
        }

        // Kết nối các điểm để tạo thành viền
        for (int i = 0; i < segmentCount * 2; i += 2)
        {
            indices.Add(i);
            indices.Add(i + 1);
            indices.Add(i + 3);

            indices.Add(i);
            indices.Add(i + 3);
            indices.Add(i + 2);
        }

        vh.AddUIVertexStream(vertices, indices);
    } 

    private UIVertex CreateVertex(Vector2 position, Color color)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;
        vertex.position = position;
        vertex.uv0 = new Vector2(position.x / rectTransform.rect.width, position.y / rectTransform.rect.height);
        return vertex;
    }

    public void SetSegments(int newSegments)
    {
        segments = Mathf.Clamp(newSegments, 3, 100);
        SetVerticesDirty();
    }

    public void SetFillPercent(float amount)
    {
        fillPercent = Mathf.Clamp01(amount);
        SetVerticesDirty();
    }

    public void SetThickness(float thick)
    {
        thickness = Mathf.Max(0, thick);
        SetVerticesDirty();
    }

    public void SetRotation(float angle)
    {
        rotationAngle = angle % 360;
        SetVerticesDirty();
    } 
}
