#region

using CustomHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Items;
using Items.ItemData;
using NaughtyAttributes;
using UnityEngine;

#endregion

[Serializable]
public struct LootDrop
{
    public int expDrop { get; private set; }
    
    [field: SerializeReference]
    public ItemGold moneyDrop { get; private set; }

    [field: SerializeReference]
    public List<Item> itemsDrop { get; private set; }

    public LootDrop(int expDrop_, ItemGold moneyDrop_, List<Item> items_)
    {
        expDrop = expDrop_;
        moneyDrop = moneyDrop_;
        itemsDrop = items_;
    }
}

[CreateAssetMenu(menuName = "ScriptableObjects/Loot Table")]
public class LootTable : ScriptableObject
{
    [MinMaxSlider(0, 1000)] [SerializeField] private Vector2Int possibleExperienceDrop;
    [MinMaxSlider(0, 1000)] [SerializeField] private Vector2Int possibleMoneyDrop;
    [MinMaxSlider(0, 10)] [SerializeField] private Vector2Int possibleItemCount;
    
    [SerializedDictionary("Item Data", "X = Min Count, Y = Max Count (for random range)")]
    [SerializeField] private SerializedDictionary<ItemData, Vector2Int> guaranteedItemDrop;
    [SerializeField] private WeightedDictionary<RarityType> rarityProbability = new WeightedDictionary<RarityType>();
    [SerializeField] private WeightedDictionary<ItemData> itemProbability = new WeightedDictionary<ItemData>();

    private ItemDatabase itemDatabase;
    [ContextMenu("Force Initialize")]
    private void OnEnable()
    {
        itemProbability.ForceInitialize();
    }

    private void OnValidate()
    {
        itemProbability.ForceInitialize();
    }

    public LootDrop GetDrop(ItemDatabase itemDatabase_)
    {
        itemDatabase = itemDatabase_;
        int _exp = possibleExperienceDrop.GetRandomInRange();
        int _moneyAmount = possibleMoneyDrop.GetRandomInRange();
        int _itemsCount = possibleItemCount.GetRandomInRange();

        var _moneyDrop = new ItemGold(itemDatabase_.GoldItemData, _moneyAmount);
        
        var _itemDrops = guaranteedItemDrop.Select(itemPair_ => InitializeItem(itemPair_.Key)).ToList();

        for (var i = 0; i < _itemsCount; i++)
        {
            var _item = InitializeItem(itemProbability.GetWeightedRandom());
            
            if (_item is not ItemStackable)
            {
                _itemDrops.Add(_item);
                continue;
            }
            
            var _similarItem = _itemDrops.FirstOrDefault(i => i.Data.ItemID == _item.Data.ItemID);
            
            if (_similarItem == null)
            {
                _itemDrops.Add(_item);
                continue;
            }
            
            var _consumableItem = (ItemConsumable) _item;
            var _similarConsumable = (ItemConsumable) _similarItem;
            _similarConsumable.AddStack(_consumableItem.StackCount);
        }
        
        return new LootDrop(_exp, _moneyDrop, _itemDrops);
    }

    private Item InitializeItem(ItemData itemData_)
    {
        return itemData_.ItemType switch
        {
            ItemType.Weapon => GetWeaponItem((WeaponData) itemData_),
            ItemType.Armor => GetArmorItem((ArmorData) itemData_),
            ItemType.Consumable => GetConsumableItem((ConsumableData) itemData_),
            _ => null
        };
    }

    private ItemWeapon GetWeaponItem(WeaponData weaponData_)
    {
        var dmg = weaponData_.GetRandomWeaponDamage();
        
        RarityType _rarity = rarityProbability.GetWeightedRandom();
        
        dmg = Mathf.RoundToInt(dmg * itemDatabase.RarityModiferValue[_rarity]);
        
        return new ItemWeapon(weaponData_, dmg, _rarity);
    }

    private ItemArmor GetArmorItem(ArmorData armorData_)
    {
        var armVal = armorData_.GetRandomArmorValue();
        
        RarityType _rarity = rarityProbability.GetWeightedRandom();
        
        armVal = Mathf.RoundToInt(armVal * itemDatabase.RarityModiferValue[_rarity]);
        
        return new ItemArmor(armorData_, armVal, _rarity);
    }

    private ItemConsumable GetConsumableItem(ConsumableData consumableData_)
    {
        var count = UnityEngine.Random.Range(1, consumableData_.maxPossibleDropCount + 1);
        return new ItemConsumable(consumableData_, count);
    }

    private List<Item> Organize(List<Item> itemList_)
    {
        var _itemsToRemove = new HashSet<Item>();
        
        foreach (var _item in itemList_)
        {
            if(_item is not ItemStackable) continue;
            if(_itemsToRemove.Contains(_item)) continue;
            
            foreach (var _otherItem in itemList_)
            {
                if(_otherItem is not ItemStackable) continue;
                if(_otherItem == _item) continue;
                if(_item.Data.ItemID != _otherItem.Data.ItemID) continue;
                if(_itemsToRemove.Contains(_otherItem)) continue;

                var _consumable = (ItemConsumable)_item;
                var _otherConsumable = (ItemConsumable) _otherItem;
                
                _consumable.AddStack(_otherConsumable.StackCount);
                _itemsToRemove.Add(_otherItem);
            }
        }

        itemList_.RemoveAll(_itemsToRemove.Contains);

        return itemList_;
    }
}