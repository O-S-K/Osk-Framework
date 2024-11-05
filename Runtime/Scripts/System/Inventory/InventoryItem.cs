using UnityEngine;

namespace OSK
{
    [System.Serializable]

    public class InventoryItem
    {
        public string name;
        public string description;
        public Sprite icon;
        public int quantity;
        public ItemType itemType; 

        public InventoryItem(string name, string desc, Sprite iconSprite, int qty, ItemType type)
        {
            this.name = name;
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