using CustomEvent;
using UnityEngine;
using Character;

public class StatusEffect : MonoBehaviour
{
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public int turnDuration { get; private set; }
    [field: SerializeField] public bool isStackable { get; private set; }
    public StatusEffectReceiver Target { get; private set; }
    public GameObject Source { get; private set; }
    public int turns { get; private set; }

    public bool HasDuration => turnDuration > 0;

    protected bool isActivated;
    
    public Evt<StatusEffect, StatusEffectReceiver> OnEffectEnd = new Evt<StatusEffect, StatusEffectReceiver>();

    protected virtual void OnActivate() { }
    protected virtual void OnDeactivate() { }

    public void Activate(StatusEffectReceiver target, GameObject source = null)
    {
        if (isActivated) return;
        Target = target;
        Source = source;
        isActivated = true;
        OnActivate();
    }

    public void Deactivate()
    {
        if (isActivated)
        {
            OnDeactivate();
            OnEffectEnd.Invoke(this, Target);
        }
        isActivated = false;
        Destroy(gameObject);
    }

    public void RefreshStatusEffect() { turns = 0; }

    public void Tick()
    {
        if (turns >= turnDuration)
        {
            RemoveFromReceiver();
        }
        OnTick();
        turns++;
    }
    
    protected virtual void OnTick() { }

    public void RemoveFromReceiver()
    {
        Target.RemoveStatusEffect(this);
    }
}