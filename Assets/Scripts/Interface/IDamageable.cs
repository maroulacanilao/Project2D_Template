using System.Collections;
using System.Collections.Generic;
using CustomEvent;
using UnityEngine;

public interface IDamageable
{
    public Evt<DamageInfo> OnDamage { get; set; }
}