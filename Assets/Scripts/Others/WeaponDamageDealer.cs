using System;
using System.Collections.Generic;
using Character;
using CustomHelpers;
using NaughtyAttributes;
using UnityEngine;

public class WeaponDamageDealer : MonoBehaviour
{
    [System.Serializable]
    public class AttackEventProperties
    {
        public string attackEventName;
        [HideInInspector] public int attackEventHash;
        public Vector2 offset;
        public float boxSize;
        public bool showBox;
        [ShowIf("showBox")] public Color boxColor;
    }
    
    [SerializeField] AnimationEventInvoker animationEventInvoker;
    [SerializeField] private LayerMask damageableLayer;
    [SerializeField] private AttackEventProperties[] attackEvents;
    private float xScaleModifier => Mathf.Sign(transform.parent.localScale.x);
    private Collider2D ownerCol;
    private void Awake()
    {
        ownerCol = transform.parent.GetComponent<Collider2D>();
        animationEventInvoker.OnAnimationEvent.AddListener(AnimEvent);

        foreach (var ae_ in attackEvents)
        {
            ae_.attackEventHash = ae_.attackEventName.ToHash();
        }
    }

    private void AnimEvent(string eventId_)
    {
        int _eventIdHash = eventId_.ToHash();
        
        foreach (var ae_ in attackEvents)
        {
            if(_eventIdHash != ae_.attackEventHash) continue;
            OnAttack(ae_);
            return;
        }
    }
    
    private void OnAttack(AttackEventProperties attackProperties_)
    {
        var _cols = Physics2D.OverlapBoxAll(transform.position + GetOffsetOrientation(attackProperties_.offset), attackProperties_.boxSize * Vector2.one, 0,damageableLayer);
        
        foreach (var _c in _cols)
        {
            if(_c == ownerCol) continue;

            if(!_c.TryGetComponent(out IDamageable _damageable)) continue;
            DamageInfo _damageInfo = new DamageInfo(10, ownerCol.gameObject);
            _damageable.OnDamage.Invoke(_damageInfo);
        }
    }
    
    Vector3 GetOffsetOrientation(Vector3 _offset)
    {
        _offset.x *= xScaleModifier;
        return _offset;
    }
    
    private void OnDrawGizmos()
    {
        if(attackEvents == null || attackEvents.Length == 0) return;
        
        foreach (var ae_ in attackEvents)
        {
            if(!ae_.showBox) continue;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position + GetOffsetOrientation(ae_.offset), ae_.boxSize * Vector2.one);
        }
    }
}