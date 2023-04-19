using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomEvent;

public class ManaComponent : MonoBehaviour
{
    [field: Header("Properties")]
    [field: SerializeField] public int MaxMana { get; private set; }
    public int CurrentMana { get; private set; }
    

    [field: Header("Events")]
    public readonly Evt<ManaComponent> OnUseMana = new Evt<ManaComponent>();
    public readonly Evt<ManaComponent> OnAddMana = new Evt<ManaComponent>();
    public readonly Evt<ManaComponent> OnNotEnoughMana = new Evt<ManaComponent>();

    private void Start()
    {
        CurrentMana = MaxMana;
    }
    
    public bool HasEnoughMana(int manaToCompare_)
    {
        return CurrentMana >= manaToCompare_;
    }
    
    private void SetCurrentMana(int newCurrMana_)
    {
        CurrentMana = Mathf.Clamp(
            newCurrMana_,
            0,
            MaxMana);
    }
    
    public void SetMaxMana(int newMaxMana_, bool willScaleCurrMana = false)
    {
        var scale = newMaxMana_ / MaxMana;
        
        MaxMana = newMaxMana_;

        if (willScaleCurrMana) SetCurrentMana(CurrentMana * scale);
    }

    /// <summary>
    /// return if there is enough mana and subtracts the mana used
    /// </summary>
    /// <param name="manaUsed_"></param>
    /// <returns></returns>
    public bool UseMana(int manaUsed_)
    {
        if (!HasEnoughMana(manaUsed_))
        {
            OnNotEnoughMana.Invoke(this);
            return false;
        }
        SetCurrentMana(CurrentMana - manaUsed_);
        
        OnUseMana.Invoke(this);
        return true;
    }

    public void AddMana(int manaAdd_)
    {
        SetCurrentMana(CurrentMana + manaAdd_);
        OnAddMana.Invoke(this);
    }
}
