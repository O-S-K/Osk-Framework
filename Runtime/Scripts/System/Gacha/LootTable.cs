using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OSK
{
// https://github.com/Balastrong/random-loot-table-video
    [CreateAssetMenu(fileName = "LootTable", menuName = "OSK/Gacha/LootTable")]
    public class LootTable : ScriptableObject
    {
        [SerializeField] private List<RewardItem> _items;
        [System.NonSerialized] private bool isInitialized = false;

        private float _totalWeight;

        private void Initialize()
        {
            if (!isInitialized)
            {
                _totalWeight = _items.Sum(item => item.weight);
                isInitialized = true;
            }
        }


        private void AltInitialize()
        {
            if (!isInitialized)
            {
                _totalWeight = 0;
                foreach (var item in _items)
                {
                    _totalWeight += item.weight;
                }

                isInitialized = true;
            }
        }


        public RewardItem GetRandomItem()
        {
            Initialize();
            float diceRoll = Random.Range(0f, _totalWeight);

            foreach (var item in _items)
            {
                if (item.weight >= diceRoll)
                {
                    return item;
                }

                diceRoll -= item.weight;
            }

            throw new System.Exception("Reward generation failed!");
        }
    }

    [System.Serializable]
    public class RewardItem
    {
        public string itemName;
        public float weight;
        public Sprite sprite;
    }
}