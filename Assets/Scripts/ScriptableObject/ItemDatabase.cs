using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Items;
using Items.ItemData;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Persistent/ItemDatabase", fileName = "New ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    [field: SerializeField] public SerializedDictionary<string, ItemData> ItemDataDictionary { get; private set; }
    
    [field: SerializeField] public SerializedDictionary<ItemType, Sprite> ItemIconDictionary { get; private set; }
    
    [field: SerializeField] public SerializedDictionary<RarityType, float> RarityModiferValue { get; private set; }
    
    [field: SerializeField] public ItemData GoldItemData { get; private set; }


    [ContextMenu("Set Item Dictionary")]
    private void SetItemDictionary()
    {
        var _datas = Resources.LoadAll<ItemData>("Test/");
        ItemDataDictionary = new SerializedDictionary<string, ItemData>();
        foreach (var _itemData in _datas)
        {
            ItemDataDictionary.Add(_itemData.ItemID,_itemData);
        }
    }
}