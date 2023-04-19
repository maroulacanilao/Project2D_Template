using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;

public interface IConsumable : IStorable
{
    public BattleStats bonusStats { get; set; }
    public void OnConsume(CharacterStatsData characterStatsData_);
}
