using UnityEngine;

namespace OSK
{
    public static class LayerUtils
    {
        // Get Sorting order to set SpriteRenderer sortingOrder, higher position = lower sortingOrder
        public const int sortingOrderDefault = 5000;
        public static int GetSortingOrder(Vector3 position, int offset, int baseSortingOrder = sortingOrderDefault)
        {
            return (int)(baseSortingOrder - position.y) + offset;
        }

        public static void SetLayer(this GameObject gameObject, int layer, bool applyToChildren = false)
        {
            gameObject.layer = layer;
            if (applyToChildren)
            {
                for (int i = 0; i < gameObject.transform.childCount; i++)
                {
                    SetLayer(gameObject.transform.GetChild(i).gameObject, layer, true);
                }
            }
        }

        public static void SetLayer(this GameObject gameObject, string nameLayer)
        {
            gameObject.layer = LayerMask.NameToLayer(nameLayer);
        }
        
        // LAYER
        public static void SetLayerAllChildren(this GameObject gameobject, string nameLayer)
        {
            foreach (GameObject child in gameobject.GetComponentsInChildren<GameObject>())
            {
                child.layer = LayerMask.NameToLayer(nameLayer);
            }
        }
    }
}