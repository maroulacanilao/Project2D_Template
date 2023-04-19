using NaughtyAttributes;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Inventory Item Data")]
    public class InventoryItemData : ScriptableObject
    {
        [field: SerializeField] public string itemID { get; private set; }
        
        [field: SerializeField] public string itemName { get; private set; }
        
        [field: ResizableTextArea]
        [field: SerializeField] public string itemDescription { get; private set; }
        
        [field: SerializeField] public bool isStackable { get; private set; } = true;
        
        [field: ShowIf("isStackable")]
        [field: SerializeField] public int maxStack { get; private set; } = 999;
        
        [field: ShowAssetPreview]
        [field: SerializeField] public Sprite icon { get; private set; }
        
        [field: ShowAssetPreview]
        [field: SerializeField] public IStorable prefab { get; private set; }

        public ItemType ItemType => prefab.ItemType;
    }
}