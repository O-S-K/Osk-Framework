using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhysicsUtils
{ 
    public static bool IsInLayerMask(GameObject gameObject, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << gameObject.layer));

        //example
        //private void OnTriggerEnter(Collider otherCollider)
        //{
        //    // if we're not in the layer, return
        //    if (!PhysicsHelper.IsInLayerMask(otherCollider.gameObject, _layersDetected)) { return; }
        //}
    } 

    public static List<RaycastHit2D> CheckCollisions(Collider2D collider, Vector2 direction, float distance)
    {
        // setup collision filter to minimize contacts
        ContactFilter2D filter = new ContactFilter2D() { };
        filter.useTriggers = false;
        filter.SetLayerMask(Physics2D.GetLayerCollisionMask(collider.gameObject.layer));
        filter.useLayerMask = true;

        // prepare our hit storage
        RaycastHit2D[] hitBuffer = new RaycastHit2D[10];
        List<RaycastHit2D> hits = new List<RaycastHit2D>();

        // look for hits and store
        int hitCount = collider.Cast(direction, filter, hitBuffer, distance);
        for (int i = 0; i < hitCount; i++)
        {
            hits.Add(hitBuffer[i]);
        }

        return hits;
    }

    public static Vector2 ReverseVector(Vector2 start, Vector2 end)
    {
        Vector2 reverseVector = (start - end) * -1;
        reverseVector.Normalize();

        return reverseVector;
    }

    public static Vector3 ReverseVector(Vector3 start, Vector3 end)
    {
        Vector3 reverseVector = (start - end) * -1;
        reverseVector.Normalize();

        return reverseVector;
    }
}
