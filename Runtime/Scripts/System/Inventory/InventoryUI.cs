using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
    public class InventoryUI : MonoBehaviour
    {
        public InventorySystem InventorySystem;
        public GameObject itemPrefab; // Prefab for displaying an item in the UI
        public Transform itemsParent; // Parent object for the item UI elements

        public void UpdateInventoryUI()
        {
            // Clear previous items
            foreach (Transform child in itemsParent)
            {
                Destroy(child.gameObject);
            }

            // Add items to the UI
            foreach (var item in InventorySystem.GetAllItems())
            {
                GameObject itemGO = Instantiate(itemPrefab, itemsParent);
                itemGO.GetComponent<ItemUI>().Setup(item);
            }
        }
    }
}