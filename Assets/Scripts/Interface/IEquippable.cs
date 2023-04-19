using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;

public interface IEquippable : IStorable
{
    public BattleStats equipmentStats { get; set; }
    public void OnEquip(CharacterStatsData characterStatsData_);
    public void OnUnEquip(CharacterStatsData characterStatsData_);
}