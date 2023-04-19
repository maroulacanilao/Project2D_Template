using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;

public class HealthConsumable : MonoBehaviour, IConsumable
{
    [field: SerializeField] public BattleStats bonusStats { get; set; }
    [field: SerializeField] public ItemType ItemType { get; set; }
    [field: SerializeField] public int healAmount { get; private set; }
    [field: SerializeField] public bool canOverHeal { get; private set; }
    
    public void OnConsume(CharacterStatsData characterStatsData_)
    {
        
    }
}
