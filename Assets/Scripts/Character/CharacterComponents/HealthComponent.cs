using CustomEvent;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [field: Header("Properties")]
    [field: SerializeField] public int MaxHp { get; private set; }

    public bool IsInvincible { get; set; }
    public bool CanOverHeal { get; set; }
    public int CurrentHp { get; private set; }

    [field: Header("Events")]
    public readonly Evt<HealthComponent,DamageInfo> OnTakeDamage = new Evt<HealthComponent,DamageInfo>();
    public readonly Evt<HealthComponent,DamageInfo> OnDeath = new Evt<HealthComponent,DamageInfo>();
    public readonly Evt<HealthComponent,HealInfo> OnHeal = new Evt<HealthComponent,HealInfo>();
    public readonly Evt<HealthComponent,HealInfo> OnMaxHp = new Evt<HealthComponent,HealInfo>();
    public readonly Evt<HealthComponent> OnMaxHpUpdate = new Evt<HealthComponent>();

    private void Start()
    {
        CurrentHp = MaxHp;
    }
    
    private void SetCurrentHP(int newCurrentHP_)
    {
        CurrentHp = Mathf.Clamp(
            newCurrentHP_,
            0,
            CanOverHeal ? int.MaxValue : MaxHp);

        if (CurrentHp <= MaxHp) CanOverHeal = false;
    }
    
    public void SetMaxHP(int newMaxHP_, bool willScaleCurrentHP = false)
    {
        var scale = newMaxHP_ / MaxHp;
        
        MaxHp = newMaxHP_;

        if (willScaleCurrentHP) SetCurrentHP(CurrentHp * scale);

        OnMaxHpUpdate.Invoke(this);
    }

    public void TakeDamage(DamageInfo damageInfo)
    {
        SetCurrentHP(CurrentHp - damageInfo.DamageAmount);
        
        OnTakeDamage.Invoke(this, damageInfo);
        
        if (CurrentHp > 0) return;
        
        OnDeath.Invoke(this, damageInfo);
    }

    public void ReceiveHeal(HealInfo healInfo, bool canOverHeal_ = false)
    {
        CanOverHeal = canOverHeal_;
        
        SetCurrentHP(CurrentHp + healInfo.HealAmount);
        
        OnHeal.Invoke(this, healInfo);

        if (CurrentHp < MaxHp) return;

        OnMaxHp.Invoke(this, healInfo);
    }
}
