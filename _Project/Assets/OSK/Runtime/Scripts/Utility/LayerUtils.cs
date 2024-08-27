using UnityEngine;

namespace OSK.Utils
{
    public static class LayerUtils
    {
        // Get Sorting order to set SpriteRenderer sortingOrder, higher position = lower sortingOrder
        public const int sortingOrderDefault = 5000;
        public static int GetSortingOrder(Vector3 position, int offset, int baseSortingOrder = sortingOrderDefault)
        {
            return (int)(baseSortingOrder - position.y) + offset;
        }

    }
}