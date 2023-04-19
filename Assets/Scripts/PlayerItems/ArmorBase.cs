using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;

public class ArmorBase : MonoBehaviour, IEquippable
{
    [field: SerializeField] public ItemType ItemType { get; set; }
    [field: SerializeField] public BattleStats equipmentStats { get; set; }


    public void OnEquip(CharacterStatsData characterStatsData_)
    {
        characterStatsData_.AddEquipmentStats(this);
    }

    public void OnUnEquip(CharacterStatsData characterStatsData_)
    {
        characterStatsData_.RemoveEquipmentStats(this);
    }

}
