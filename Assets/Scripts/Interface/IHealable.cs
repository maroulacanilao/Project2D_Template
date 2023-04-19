using System.Collections;
using System.Collections.Generic;
using CustomEvent;
using UnityEngine;

public interface IHealable
{
    public Evt<HealInfo, bool> OnHeal { get; set; }
}
