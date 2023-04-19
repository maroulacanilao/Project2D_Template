using System.Collections;
using System.Collections.Generic;
using Stats;
using UnityEngine;

public class WeaponBase : MonoBehaviour, IEquippable, IUsable
{
    public BattleStats equipmentStats { get; set; }
    [field: SerializeField] public ItemType ItemType { get; set; }
    [field: SerializeField] public BattleStats weaponStats { get; private set; }
    
    public void OnEquip(CharacterStatsData characterStatsData_)
    {
        
    }
    
    public void OnUnEquip(CharacterStatsData characterStatsData_)
    {
        
    }
    
    public void OnUse()
    {
        
    }
}
