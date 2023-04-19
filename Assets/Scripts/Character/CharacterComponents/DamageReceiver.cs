using System;
using Stats;
using UnityEngine;
using CustomHelpers;

public enum AttackResult
{
    Miss,
    Block,
    Hit,
    Critical
}

public class DamageReceiver : MonoBehaviour
{
    private CharacterBase owner;
    private CharacterStatsData statsData;
    private HealthComponent healthComponent;
    
    private void Awake()
    {
        owner = GetComponent<CharacterBase>();
        statsData = owner.statsData;
        healthComponent = owner.healthComponent;
    }
    
    private AttackResult TakeDamage(DamageInfo damageInfo_)
    {
        if (IsEvaded(damageInfo_.Source)) return AttackResult.Miss;
        
        var res = statsData.DamageTypeResistance[damageInfo_.DamageType];
        float dmgTypeMult = 1 - res;
        float armorDmgMult = GetArmorDamageMultiplier();
        
        float finalDmg = damageInfo_.DamageAmount * dmgTypeMult * armorDmgMult;
        damageInfo_.DamageAmount = Mathf.RoundToInt(finalDmg);
        healthComponent.TakeDamage(damageInfo_);
        Debug.Log($"{gameObject.name} was damaged for {damageInfo_.DamageAmount}. HP: {healthComponent.CurrentHp}");
        return AttackResult.Hit;
    }
    
    private float GetArmorDamageMultiplier()
    {
        return 1 - ((0.052f * statsData.totalStats.armor) 
                    / (0.9f + 0.048f * Math.Abs(statsData.totalStats.armor)));
    }
    
    private bool IsEvaded(GameObject attacker)
    {
        if (!attacker.TryGetComponent(out CharacterBase _characterBase)) return false;

        var attackerAcc = _characterBase.statsData.totalStats.speed + _characterBase.statsData.totalStats.accuracy;
        var totalSum = 1 + (attackerAcc * 2) + statsData.totalStats.speed;
        return MathHelper.RandomBool(statsData.totalStats.speed / totalSum);
    }
}
