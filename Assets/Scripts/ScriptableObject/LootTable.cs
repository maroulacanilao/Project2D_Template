#region

using Inventory;
using NaughtyAttributes;
using UnityEngine;

#endregion

public struct LootDrop
{
    public int expDrop { get; private set; }
    public int moneyDrop { get; private set; }
    public InventoryItem[] itemsDrop { get; private set; }

    public LootDrop(int expDrop_, int moneyDrop_, InventoryItem[] items_)
    {
        expDrop = expDrop_;
        moneyDrop = moneyDrop_;
        itemsDrop = items_;
    }
}

[CreateAssetMenu(menuName = "ScriptableObjects/Loot Table")]
public class LootTable : ScriptableObject
{
    [MinMaxSlider(0, 5000)] [SerializeField] private Vector2Int possibleExperienceDrop;
    [MinMaxSlider(0, 5000)] [SerializeField] private Vector2Int possibleMoneyDrop;
    [MinMaxSlider(0, 10)] [SerializeField] private Vector2Int possibleItemDrop;
    [SerializeField] private WeightedDictionary<InventoryItem> itemDropDictionary = new WeightedDictionary<InventoryItem>();

    private void OnEnable()
    {
        itemDropDictionary.ForceInitialize();
    }

    public LootDrop GetDrop()
    {
        var _exp = Random.Range(possibleExperienceDrop.x, possibleExperienceDrop.y);
        var _moneyDrop = Random.Range(possibleMoneyDrop.x, possibleMoneyDrop.y);
        var _itemsCount = Random.Range(possibleItemDrop.x, possibleItemDrop.y);
        var _itemsDrop = new InventoryItem[_itemsCount];

        for (var i = 0; i < _itemsCount; i++)
        {
            _itemsDrop[i] = itemDropDictionary.GetWeightedRandom();
        }

        return new LootDrop(_exp, _moneyDrop, _itemsDrop);
    }
}