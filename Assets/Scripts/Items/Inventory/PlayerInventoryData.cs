using System.Collections.Generic;
using CustomEvent;
using CustomHelpers;
using UnityEngine;

namespace Inventory
{
    [System.Serializable]
    public class InventoryItem
    {
        public InventoryItemData data { get; private set; }
        public int stackCount { get; private set; }
        
        public int maxStack => data.maxStack;
        public bool isStackable => data.isStackable;
        public InventoryItem(InventoryItemData source_)
        {
            data = source_;
            if(isStackable) AddToStack();
        }

        public bool AddToStack()
        {
            if (!isStackable) return false;
            if (stackCount >= maxStack) return false;
            
            stackCount++;
            return true;
        }
        
        public void RemoveFromStack() => stackCount--;
    }
    
    [CreateAssetMenu(menuName = "ScriptableObjects/Persistent/PlayerInventoryData")]
    public class PlayerInventoryData : ScriptableObject
    {
        private Dictionary<InventoryItemData, InventoryItem> itemDictionary;
        public List<InventoryItem> inventory { get; private set; }
        public int maxInventorySpace { get; private set; } = 12;

        public readonly Evt<PlayerInventoryData> OnUpdateInventory = new Evt<PlayerInventoryData>();
        public readonly Evt<PlayerInventoryData> OnChangeMaxInventorySpace = new Evt<PlayerInventoryData>();

        private void InitializeInventory(int inventorySpace_)
        {
            maxInventorySpace = inventorySpace_;
            inventory = new List<InventoryItem>(maxInventorySpace);
            itemDictionary = new Dictionary<InventoryItemData, InventoryItem>(maxInventorySpace);
        }

        public void ChangeInventorySpace(int newInventoryCount)
        {
            maxInventorySpace = newInventoryCount;
            var _prevInventory = inventory;
            var _prevDictionary = itemDictionary;
            
            inventory = new List<InventoryItem>(maxInventorySpace);
            inventory.AddRange(_prevInventory);
            
            itemDictionary = new Dictionary<InventoryItemData, InventoryItem>(maxInventorySpace);
            itemDictionary.AddRange(_prevDictionary);
            
            OnChangeMaxInventorySpace.Invoke(this);
        }

        /// <summary>
        /// return if there is a inventory space left and the item was successfully added
        /// </summary>
        /// <param name="referenceData_"></param>
        /// <returns></returns>
        public bool AddItem(InventoryItemData referenceData_)
        {
            if (referenceData_.isStackable && 
                itemDictionary.TryGetValue(referenceData_, out var _item))
            {
                var _wasAdded = _item.AddToStack();
                if (_wasAdded) OnUpdateInventory.Invoke(this);
                return _wasAdded;
            }

            // check if there is no space left
            if (maxInventorySpace <= inventory.Count) return false;
                
            InventoryItem _newItem = new InventoryItem(referenceData_);
            inventory.Add(_newItem);
            itemDictionary.Add(referenceData_,_newItem);
            OnUpdateInventory.Invoke(this);
            return true;
        }

        public void RemoveItem(InventoryItemData referenceData_)
        {
            if (!itemDictionary.TryGetValue(referenceData_, out var _item)) return;

            _item.RemoveFromStack();
            if(_item.isStackable || _item.stackCount <= 0) itemDictionary.Remove(referenceData_);
            OnUpdateInventory.Invoke(this);
        }

        public InventoryItem GetItem(InventoryItemData referenceData_)
        {
            return itemDictionary.TryGetValue(referenceData_, out var _item) 
                ? _item : null;
        }

        public InventoryItem GetAndRemoveItem(InventoryItemData referenceData_)
        {
            var _item = GetItem(referenceData_);
            if (_item == null) return null;
            
            _item.RemoveFromStack();
            if(_item.isStackable || _item.stackCount <= 0) itemDictionary.Remove(referenceData_);
            OnUpdateInventory.Invoke(this);
            
            return _item;
        }
    }
}