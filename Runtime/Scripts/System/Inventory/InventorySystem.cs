using System.Collections.Generic;
using System.IO;
using CustomInspector;
using UnityEngine;

namespace OSK
{
    public class InventorySystem : MonoBehaviour, IService
    {
        [TableList]
        [SerializeField] private List<InventoryItem> items = new List<InventoryItem>();

        public void AddItem(InventoryItem item)
        {
            foreach (var inventoryItem in items)
            {
                if (inventoryItem.name == item.name)
                {
                    inventoryItem.quantity += item.quantity; 
                    return;
                }
            }

            if (!items.Contains(item))
                items.Add(item);
        }
        
        public void AddItems(List<InventoryItem> itemsToAdd)
        {
            foreach (var item in itemsToAdd)
                AddItem(item);
        }

        public void RemoveItem(string itemName, int quantity)
        {
            InventoryItem itemToRemove = items.Find(item => item.name == itemName);

            if (itemToRemove != null)
            {
                if (itemToRemove.quantity > quantity)
                {
                    itemToRemove.quantity -= quantity;
                }
                else
                {
                    items.Remove(itemToRemove); 
                }
            }
        }

        public InventoryItem GetItem(string itemName) => items.Find(item => item.name == itemName);
        public List<InventoryItem> GetAllItems() => items;
         
        public void DrawInventory()
        {
            foreach (var item in items)
            {
                Logg.Log(
                    $"Item: {item.name}, " +
                    $"Description: {item.description}, " +
                    $"Quantity: {item.quantity}, " +
                    $"Type: {item.itemType}");
            }
        }
    }
}