using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None,
    QuestItem,
    Consumable,
    Armor,
    Weapon,
    Tool,
}

public interface IStorable
{
    public ItemType ItemType { get; set; }
}
