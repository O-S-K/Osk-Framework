using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace OSK
{
    public class InventorySystem : MonoBehaviour
    {
        private List<InventoryItem> items = new List<InventoryItem>();

        public void AddItem(InventoryItem item)
        {
            // Check if the item already exists
            foreach (var inventoryItem in items)
            {
                if (inventoryItem.itemName == item.itemName)
                {
                    inventoryItem.quantity += item.quantity; // Increase quantity
                    return;
                }
            }

            items.Add(item); // Add new item if it doesn't exist
        }

        public void RemoveItem(string itemName, int quantity)
        {
            InventoryItem itemToRemove = items.Find(item => item.itemName == itemName);

            if (itemToRemove != null)
            {
                if (itemToRemove.quantity > quantity)
                {
                    itemToRemove.quantity -= quantity; // Decrease quantity
                }
                else
                {
                    items.Remove(itemToRemove); // Remove item if quantity is 0 or less
                }
            }
        }

        public InventoryItem GetItem(string itemName)
        {
            return items.Find(item => item.itemName == itemName);
        }

        public List<InventoryItem> GetAllItems()
        {
            return items;
        }

        // Method to save the inventory
        public void SaveInventory(string path)
        {
            string json = JsonUtility.ToJson(this);
            File.WriteAllText(path, json);
        }

        // Method to load the inventory
        public void LoadInventory(string path)
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                JsonUtility.FromJsonOverwrite(json, this);
            }
        }
    }
}