using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using Items.ItemData;
using UnityEngine;

public static class ItemHelper
{
    public static Item GetItem(this ItemData data_, ItemDatabase itemDatabase_, PlayerLevelData levelData_, RarityType rarityType_ = RarityType.Common)
    {
        switch (data_.ItemType)
        {
            case ItemType.Weapon:
            {
                var _weaponData = (WeaponData) data_;
                return _weaponData.GetWeaponItem(itemDatabase_, levelData_, rarityType_);
            }
            case ItemType.Armor:
            {
                var _armorData = (ArmorData) data_;
                return _armorData.GetArmorItem(itemDatabase_, levelData_, rarityType_);
            }

            case ItemType.Consumable:
            {
                var _consumableData = (ConsumableData) data_;
                return _consumableData.GetConsumableItem();
            }
            case ItemType.Gold:
            {
                return new ItemGold(data_, 0);
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public static ItemWeapon GetWeaponItem(this WeaponData data_, ItemDatabase itemDatabase_, PlayerLevelData levelData_, RarityType rarityType_)
    {
        var _baseDmg = data_.GetRandomWeaponDamage();
        var _modifiers = (1f + itemDatabase_.RarityModiferValue[rarityType_]) * (1f + ((float)levelData_.CurrentLevel / levelData_.LevelCap));
        var _weaponDmg = Mathf.RoundToInt(_baseDmg * _modifiers);
        return new ItemWeapon(data_, _weaponDmg, rarityType_);
    }

    public static ItemArmor GetArmorItem(this ArmorData data_, ItemDatabase itemDatabase_, PlayerLevelData levelData_, RarityType rarityType_)
    {
        var _baseVal = data_.GetRandomArmorValue();
        var _modifiers = (1f + itemDatabase_.RarityModiferValue[rarityType_]) * (1f + ((float)levelData_.CurrentLevel / levelData_.LevelCap));
        var _armorVal = Mathf.RoundToInt(_baseVal * _modifiers);
        return new ItemArmor(data_, _armorVal, rarityType_);
    }

    public static ItemConsumable GetConsumableItem(this ConsumableData data_)
    {
        var _count = UnityEngine.Random.Range(1, data_.maxPossibleDropCount);
        return new ItemConsumable(data_, _count);
    }

    public static ItemGold GetGoldItem(this ItemData data_, int goldAmount_)
    {
        return new ItemGold(data_, goldAmount_);
    }
}
