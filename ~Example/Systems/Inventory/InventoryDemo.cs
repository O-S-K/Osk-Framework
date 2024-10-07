using System.Collections;
using System.Collections.Generic;
using OSK;
using UnityEngine;
using UnityEngine.Serialization;

public class InventoryDemo : MonoBehaviour
{
    public InventorySystem inventorySystem;
    public InventoryUI inventoryUIManager;

    private void Start()
    {
        inventoryUIManager.InventorySystem = inventorySystem;

        // Example items
        InventoryItem apple = new InventoryItem("Apple", "A juicy apple.", null, 1, ItemType.Consumable);
        InventoryItem potion = new InventoryItem("Health Potion", "Restores health.", null, 2, ItemType.Consumable);
        inventorySystem.AddItem(apple);
        inventorySystem.AddItem(potion);
        inventorySystem.AddItem(new InventoryItem("Apple", "A juicy apple.", null, 1, ItemType.Consumable));

        inventoryUIManager.UpdateInventoryUI();
        inventorySystem.RemoveItem("Apple", 1);

        // Load inventory if needed
        string path = Application.persistentDataPath + "/inventory.json";
        inventorySystem.LoadInventory(path);
        inventoryUIManager.UpdateInventoryUI();
    }

    private void OnApplicationQuit()
    {
        // Save inventory when quitting
        string path = Application.persistentDataPath + "/inventory.json";
        inventorySystem.SaveInventory(path);
    }
}
