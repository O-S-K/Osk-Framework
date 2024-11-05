using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace OSK
{
    public class ItemUI : MonoBehaviour
    {
        public Text itemNameText;
        public Text itemQuantityText;
        public Image itemIcon;
        private InventoryItem currentItem;

        public void Setup(InventoryItem item)
        {
            currentItem = item;
            itemNameText.text = item.name;
            itemQuantityText.text = "x" + item.quantity.ToString();
            itemIcon.sprite = item.icon;
        }

        public void UseItem()
        {
            if (currentItem.itemType == ItemType.Consumable)
            {
                // Logic for using the item (e.g., healing, gaining a buff)
                Debug.Log($"Used {currentItem.name}");
                currentItem.quantity--;

                // Remove item if quantity is zero
                if (currentItem.quantity <= 0)
                {
                    //FindObjectOfType<InventorySystem>().RemoveItem(currentItem.itemName, 1);
                }

                // Update UI
                //FindObjectOfType<InventoryUI>().UpdateInventoryUI();
            }
        }
    }
}
