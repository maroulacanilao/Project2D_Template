using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvent;
using UnityEngine;

public class StatusEffectReceiver : MonoBehaviour
{
    private Dictionary<int, StatusEffect> StatusEffectsDictionary;
    private Transform container;
    
    public Evt<StatusEffect, StatusEffectReceiver> OnApply = new Evt<StatusEffect, StatusEffectReceiver>();
    public Evt<StatusEffect, StatusEffectReceiver> OnRemove = new Evt<StatusEffect, StatusEffectReceiver>();

    private void OnEnable()
    {
        container = new GameObject("StatusEffect Container").transform;
        container.parent = gameObject.transform;
        container.localPosition = Vector3.zero;
    }
    
    public bool ApplyStatusEffect(StatusEffect effect_, GameObject source_ = null)
    {
        //if stackable and status effect already in effect
        // if (effect_.isStackable &&
        //     IsDuplicateByType(effect_.GetType()))
        // {
        //     effect_.Deactivate();
        //     return false;
        // }
        
        StatusEffectsDictionary.Add(effect_.gameObject.GetInstanceID(), effect_);
        
        effect_.transform.parent = container.transform;
        
        effect_.Activate(this, source_);
        OnApply?.Invoke(effect_,this);
        return true;
    }
    
    public void RemoveStatusEffect(int effectID_)
    {
        var effect_ = StatusEffectsDictionary[effectID_];
        StatusEffectsDictionary.Remove(effectID_);
        effect_.Deactivate();
        OnRemove?.Invoke(effect_,this);
    }
    
    public void RemoveStatusEffect(StatusEffect effect_)
    {
        StatusEffectsDictionary.Remove(effect_.gameObject.GetInstanceID());
        effect_.Deactivate();
        OnRemove?.Invoke(effect_,this);
    }
    
    public void RemoveEffectByType<T>()
     where T : StatusEffect
    {
        foreach (var effect in StatusEffectsDictionary.Values.OfType<T>())
        {
            RemoveStatusEffect(effect);
        }
    }
    
    public bool IsDuplicateByType<T>()
        where T : StatusEffect
    {
        return StatusEffectsDictionary.Values.OfType<T>().Any();
    }
    
    public bool IsDuplicateByType(Type T)
    {
        return StatusEffectsDictionary.Values.Any(se_ => se_.GetType() == T);
    }
}
