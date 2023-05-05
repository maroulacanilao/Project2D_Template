using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HealInfo
{
    public readonly int HealAmount;
    public readonly GameObject Source;
    public readonly bool CanOverHeal;

    public HealInfo(int healAmount_, GameObject source_, bool canOverHeal_)
    {
        HealAmount = healAmount_;
        Source = source_;
        CanOverHeal = canOverHeal_;
    }
}