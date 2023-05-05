using System;
using NaughtyAttributes;
using UnityEngine;

namespace Items.ItemData
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ItemData/ConsumableData", fileName = "ConsumableData")]
    public class ConsumableData : ItemData
    {
        [field: SerializeField] public StatusEffect StatusEffect { get; private set; }
        
        [field: SerializeField] public bool IsStackable { get; private set; } = true;

        [field: SerializeField] [field: MinValue(1)] [field: ShowIf("IsStackable")]
        public int maxPossibleDropCount { get; private set; } = 1;

        private void Reset()
        {
            itemType = ItemType.Consumable;
        }

        private void OnValidate()
        {
            itemType = ItemType.Consumable;
        }
    }
}