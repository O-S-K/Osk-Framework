using UnityEngine;

namespace OSK
{
    [System.Serializable]

    public class InventoryItem
    {
        public string itemName;
        public string description;
        public Sprite icon;
        public int quantity;
        public ItemType itemType; // New field for item type

        public InventoryItem(string name, string desc, Sprite iconSprite, int qty, ItemType type)
        {
            itemName = name;
            description = desc;
            icon = iconSprite;
            quantity = qty;
            itemType = type;
        }
    }

    public enum ItemType
    {
        Consumable,
        Equipment,
        QuestItem,
        Miscellaneous
    }
}