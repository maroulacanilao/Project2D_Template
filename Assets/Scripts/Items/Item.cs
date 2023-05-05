using System;
using Character;
using Items.ItemData;

namespace Items
{
    public enum ItemType
    {
        None = 0,
        Weapon = 1,
        Armor = 2,
        Consumable = 3,
        QuestItem = 4,
        Tool = 5,
        Gold = 6
    }

    public enum RarityType
    {
        Common = 0,
        Uncommon = 1,
        Rare = 2,
        Epic = 3,
    }

    [Serializable]
    public abstract class Item
    {
        protected ItemData.ItemData data;
        protected RarityType rarityType;
        protected string dataID;
        protected int sellValue;
        
        public ItemData.ItemData Data => data;
        public ItemType ItemType => Data.ItemType;
        public RarityType RarityType => rarityType;
        public int SellValue => sellValue;

        protected Item(ItemData.ItemData data_, RarityType rarityType_ = RarityType.Common)
        {
            data = data_;
            dataID = data_.ItemID;
            rarityType = rarityType_;
            float tempVal = data_.ItemBaseValue * (1 + ((int) rarityType_ / 10f));
            sellValue = UnityEngine.Mathf.RoundToInt(tempVal);
        }
    }

    [Serializable]
    public abstract class ItemGear : Item
    {
        protected CombatStats stats;
        protected bool isEquipped;
        
        public CombatStats Stats => stats;
        public bool IsEqquiped => isEquipped;

        protected ItemGear(ItemData.ItemData data_ , RarityType rarityType_) : base(data_, rarityType_)
        {
            data = data_;
            isEquipped = false;
        }

        public virtual void OnEquip(CharacterBase character_)
        {
            character_.statsData.AddEquipmentStats(stats, this);
            isEquipped = true;
        }

        public virtual void OnUnEquip(CharacterBase character_)
        {
            character_.statsData.RemoveEquipmentStats(stats, this);
            isEquipped = false;
        }
    }

    public abstract class ItemStackable : Item
    {
        protected int stackCount;
        
        public int StackCount => stackCount;
        public bool HasStack => stackCount > 0;
        
        protected ItemStackable(ItemData.ItemData data_, int count_, RarityType rarityType_ = RarityType.Common) 
            : base(data_, rarityType_)
        {
            data = data_;
            stackCount = count_;
        }
        
        public void AddStack()
        {
            stackCount++;
        }

        public void AddStack(int amount_)
        {
            stackCount += amount_;
        }

        /// <summary>
        /// returns true if there is still stack
        /// </summary>
        /// <returns></returns>
        public bool RemoveStack()
        {
            stackCount--;
            return HasStack;
        }

        public bool RemoveStack(int amount_)
        {
            stackCount -= amount_;
            return HasStack;
        }

        public void ClearStack()
        {
            stackCount = 0;
        }
        
    }

    [Serializable]
    public class ItemWeapon : ItemGear
    {
        public ItemWeapon(WeaponData data_, int damage_, RarityType rarityType_) : base(data_, rarityType_)
        {
            stats = new CombatStats()
            {
                weaponDamage = damage_
            };
        }
    }

    [Serializable]
    public class ItemArmor : ItemGear
    {
        public ItemArmor(ArmorData data_, int armorValue_, RarityType rarityType_) : base(data_, rarityType_)
        {
            stats = new CombatStats()
            {
                armor = armorValue_
            };
        }
    }

    [Serializable]
    public class ItemConsumable : ItemStackable
    {
        public ItemConsumable(ItemData.ItemData data_, int count_, RarityType rarityType_ = RarityType.Common) : base(data_, count_, rarityType_)
        {
            
        }
        
        /// <summary>
        /// returns true if there is still stack available
        /// </summary>
        /// <returns></returns>
        public bool Consume(CharacterBase characterBase_)
        {
            var _data = (ConsumableData) data;
            characterBase_.statusEffectReceiver.ApplyStatusEffect(_data.StatusEffect);

            RemoveStack();
            return HasStack;
        }
    }

    public class ItemGold : Item
    {
        private int goldAmount;
        public int GoldAmount => goldAmount;

        public ItemGold(ItemData.ItemData data_, int amount_, RarityType rarityType_ = RarityType.Common) : base(data_, rarityType_)
        {
            goldAmount = amount_;
        }

        public void AddGold(int amount_)
        {
            goldAmount += amount_;
        }

        /// <summary>
        /// returns true if there is enough gold to be removed
        /// </summary>
        /// <param name="amount_"></param>
        /// <returns></returns>
        public bool RemoveGold(int amount_)
        {
            if (amount_ > goldAmount) return false;

            goldAmount -= amount_;
            return true;
        }
    }

    public class ItemSeed : ItemStackable
    {
        public ItemSeed(ItemData.ItemData data_, int count_, RarityType rarityType_ = RarityType.Common) : base(data_, count_, rarityType_)
        {
            
        }

        public ItemData.ItemData UseSeed()
        {
            stackCount--;
            return data;
        }
    }
}