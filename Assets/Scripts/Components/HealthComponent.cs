using System;
using CustomEvent;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamagable, IHealable
{
    [field: Header("Properties")]
    [field: SerializeField] public float MaxHp { get; private set; }
    public bool IsInvincible { get; set; }
    public bool CanOverHeal { get; set; }
    public float CurrentHp { get; private set; }
    
    [field: Header("Events")]
    public readonly Evt<DamageInfo> OnTakeDamage = new Evt<DamageInfo>();
    public readonly Evt<DamageInfo> OnDeath = new Evt<DamageInfo>();
    public readonly Evt<HealInfo> OnHeal = new Evt<HealInfo>();
    public readonly Evt<HealInfo> OnMaxHp = new Evt<HealInfo>();


    private void Start()
    {
        CurrentHp = MaxHp;
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        if (!IsInvincible)
        {
            CurrentHp = Mathf.Clamp(
                CurrentHp - damageInfo.Damage,
                0,
                CanOverHeal ? float.MaxValue : MaxHp);
        }
        
        OnTakeDamage.Invoke(damageInfo);
        
        if (CurrentHp > 0) return;
        
        OnDeath.Invoke(damageInfo);
    }

    public void ReceiveHeal(HealInfo healInfo)
    {
        CurrentHp = Mathf.Clamp(
            CurrentHp - healInfo.HealAmount,
            0,
            CanOverHeal ? float.MaxValue : MaxHp);
        
        OnHeal.Invoke(healInfo);
        
        if(CurrentHp < MaxHp) return;
        
        OnMaxHp.Invoke(healInfo);
    }
}
