using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvent;
using Stats;
using UnityEngine;

[RequireComponent(typeof(StatusEffectReceiver))]
public class CharacterBase : MonoBehaviour, IDamagable, IHealable
{
    [field: SerializeField] public DamageType damageType { get; protected set; }
    [field: SerializeField] public HealthComponent healthComponent { get; private set; }
    [field: SerializeField] public ManaComponent manaComponent { get; private set; }
    [field: SerializeField] public StatusEffectReceiver statusEffectReceiver { get; protected set; }
    [field: SerializeField] public CharacterStatsData statsData { get; private set; }
    
    public Evt<DamageInfo> OnDamage { get; set; } = new Evt<DamageInfo>();
    public Evt<HealInfo, bool> OnHeal { get; set; } = new Evt<HealInfo, bool>();

    private void OnEnable()
    {
        OnDamage.AddListener(TakeDamage);
        OnHeal.AddListener(healthComponent.ReceiveHeal);
    }

    private void OnDisable()
    {
        OnDamage.RemoveListener(TakeDamage);
        OnHeal.AddListener(healthComponent.ReceiveHeal);
    }
    
    private void TakeDamage(DamageInfo damageInfo_)
    {
        var res = statsData.DamageTypeResistance[damageInfo_.DamageType];
        float dmgTypeMult = 1 - res;
        float armorDmgMult = 1 - ((0.052f * statsData.totalStats.armor) 
                                  / (0.9f + 0.048f * Math.Abs(statsData.totalStats.armor)));
        
        float finalDmg = damageInfo_.DamageAmount * dmgTypeMult * armorDmgMult;
        damageInfo_.DamageAmount = Mathf.RoundToInt(finalDmg);
        healthComponent.TakeDamage(damageInfo_);
        Debug.Log($"{gameObject.name} was damaged for {damageInfo_.DamageAmount}. HP: {healthComponent.CurrentHp}");
    }
}
