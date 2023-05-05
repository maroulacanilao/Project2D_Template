using System;
using System.Collections.Generic;
using System.Linq;
using Character;
using CustomEvent;
using CustomHelpers;
using Items.ItemData;
using UnityEngine;

namespace Items.Inventory
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Persistent/InventoryData", fileName = "New InventoryData")]
    public class InventoryData : ScriptableObject
    {
        [field: SerializeField] public ItemDatabase itemDatabase { get; private set; }
        
        [SerializeField] private int maxWeaponStorageSpace = 10;
        [SerializeField] private int maxArmorStorageSpace = 10;
        [SerializeField] private int maxConsumableStorageSpace = 20;
        
        private List<ItemArmor> armorStorage;
        private List<ItemWeapon> weaponStorage;
        private List<Item> miscStorage;
        private Dictionary<ItemData.ItemData, ItemConsumable> consumableStorage;
        
        private ItemArmor currArmor;
        private ItemWeapon currWeapon;
        private ItemGold gold;
        private PlayerCharacter player;

        public static readonly Evt<InventoryData> OnUpdateInventory = new Evt<InventoryData>();
        public static readonly Evt<InventoryData> OnWeaponEquip = new Evt<InventoryData>(); 
        public static readonly Evt<InventoryData> OnArmorEquip = new Evt<InventoryData>(); 

        #region Public Variables
        
        public List<ItemArmor> ArmorStorage => armorStorage;
        public List<ItemWeapon> WeaponStorage => weaponStorage;
        public List<ItemConsumable> ConsumableStorage => consumableStorage.Values.ToList();
        public List<Item> MiscStorage => miscStorage;
        public PlayerCharacter PlayerCharacter => player;
        
        public ItemGold Gold => gold;
        public ItemArmor ArmorEquipped => currArmor;
        public ItemWeapon WeaponEquipped => currWeapon;

        public bool HasEnougWeaponStorageSpace => maxWeaponStorageSpace >= weaponStorage.Count;
        public bool HasEnougArmorStorageSpace => maxArmorStorageSpace >= armorStorage.Count;
        public bool HasEnougConsumableStorageSpace => maxConsumableStorageSpace >= consumableStorage.Count;

        #endregion

        public void Initialize(PlayerCharacter player_)
        {
            player = player_;
            gold = new ItemGold(itemDatabase.GoldItemData, 0);
            armorStorage = new List<ItemArmor>();
            weaponStorage = new List<ItemWeapon>();
            consumableStorage = new Dictionary<ItemData.ItemData, ItemConsumable>();
            miscStorage = new List<Item>();
        }

        #region Public Methods
        
        public void AddLoot(LootDrop loots_)
        {
            foreach (var _item in loots_.itemsDrop)
            {
                AddItem(_item);
            }
        }

        public bool AddItem(Item item_)
        {
            switch (item_.ItemType)
            {
                case ItemType.Weapon:
                {
                    if (!HasEnougWeaponStorageSpace) return false;
                    weaponStorage.AddItemToList<ItemWeapon>((ItemWeapon) item_, this);
                    return true;
                }
                case ItemType.Armor:
                {
                    if (!HasEnougArmorStorageSpace) return false;
                    armorStorage.AddItemToList<ItemArmor>((ItemArmor) item_, this);
                    return true;
                }
                case ItemType.Consumable:
                {
                    if (HasEnougConsumableStorageSpace) return false;
                    AddConsumable((ItemConsumable) item_);
                    return true;
                }
                case ItemType.Gold:
                    AddGold((ItemGold)item_);
                    return true;
                default:
                    miscStorage.AddItemToList<Item>(item_,this);
                    return true;
            }
        }
        
        public bool RemoveItem(Item item_)
        {
            switch (item_.ItemType)
            {
                case ItemType.Weapon:
                    return weaponStorage.RemoveItemToList<ItemWeapon>((ItemWeapon) item_, this);
                
                case ItemType.Armor:
                    return armorStorage.RemoveItemToList<ItemArmor>((ItemArmor) item_, this);
                
                case ItemType.Consumable:
                    return RemoveConsumable((ItemConsumable)item_);

                case ItemType.Gold:
                    RemoveGold((ItemGold)item_);
                    return true;
                
                default:
                    return miscStorage.RemoveItemToList(item_, this);
            }
        }
        
        public void EquipArmor(ItemArmor armor_)
        {
            currArmor?.OnUnEquip(player);
            
            if (armor_ != null && !armorStorage.Any(a => a == armor_))
            {
                armorStorage.Add(armor_);
            }
            
            currArmor = armor_;
            currArmor?.OnEquip(player);
            
            if(currArmor != null) armorStorage.MoveItemToFirst(currArmor);
            
            OnUpdateInventory.Invoke(this);
            OnArmorEquip.Invoke(this);
        }

        public void EquipWeapon(ItemWeapon weapon_)
        {
            currWeapon?.OnUnEquip(player);
            
            if (weapon_ != null && !weaponStorage.Any(w => w == weapon_))
            {
                weaponStorage.Add(weapon_);
            }

            currWeapon = weapon_;
            currWeapon?.OnEquip(player);
            
            if(currArmor != null) weaponStorage.MoveItemToFirst(currWeapon);
            
            OnUpdateInventory.Invoke(this);
            OnWeaponEquip.Invoke(this);
        }

        public void ConsumeItem(ItemConsumable consumable_)
        {
            if (!consumableStorage.TryGetValue(consumable_.Data, out var _itemConsumable))
            {
                AddConsumable(consumable_);
            }
            
            if(_itemConsumable != null && _itemConsumable.Consume(player)) return;
            
            // if item stack is 0
            RemoveItem(_itemConsumable);
        }
        
        public void AddGold(int amount_)
        {
            gold.AddGold(amount_);
        }
        
        /// <summary>
        /// returns true if enough gold to be removed
        /// </summary>
        /// <param name="amount_"></param>
        /// <returns></returns>
        public bool RemoveGold(int amount_)
        {
            return gold.RemoveGold(amount_);
        }

        public void SellItem(Item itemToSell_)
        {
            int value = itemToSell_.Data.ItemBaseValue;
            RemoveItem(itemToSell_);
            AddGold(value);
        }
        
        #endregion

        #region Private Methods

        private void AddItemToList<T>(ref List<T> itemGears_, T gearToAdd_) where T: Item
        {
            itemGears_.Add(gearToAdd_);
            OnUpdateInventory.Invoke(this);
        }

        private void AddConsumable(ItemConsumable itemConsumable_)
        {
            if (consumableStorage.TryGetValue(itemConsumable_.Data, out var _consumable))
            {
                _consumable.AddStack();
                return;
            }
            consumableStorage.Add(itemConsumable_.Data, itemConsumable_);
            OnUpdateInventory.Invoke(this);
        }

        private bool RemoveConsumable(ItemConsumable itemConsumable_)
        {
            if (!consumableStorage.ContainsKey(itemConsumable_.Data)) return false;
            consumableStorage.Remove(itemConsumable_.Data);
            return true;
        }
        
        private void AddGold(ItemGold gold_)
        {
            gold.AddGold(gold_.GoldAmount);
        }

        /// <summary>
        /// returns true if enough gold to be removed
        /// </summary>
        /// <param name="amount_"></param>
        /// <returns></returns>
        private bool RemoveGold(ItemGold gold_)
        {
            return gold.RemoveGold(gold_.GoldAmount);
        }
        #endregion
    }

    public static class InventoryHelper
    {
        public static void AddItemToList<T>(this IList<T> source, T itemToAdd, InventoryData inventoryData_)
            where T : Item
        {
            source.Add(itemToAdd);
            InventoryData.OnUpdateInventory.Invoke(inventoryData_);
        }

        public static bool RemoveItemToList<T>(this IList<T> source, T itemToRemove_, InventoryData inventoryData_)
            where T : Item
        {
            if(source.Remove(itemToRemove_)) return false;
            InventoryData.OnUpdateInventory.Invoke(inventoryData_);
            return true;
        }
    }
}