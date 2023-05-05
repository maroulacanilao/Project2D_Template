using System;
using System.Collections.Generic;
using System.Linq;
using CustomEvent;
using UnityEngine;

namespace Character
{
    public class StatusEffectReceiver : MonoBehaviour
    {
        private Transform container;

        public Evt<StatusEffect, StatusEffectReceiver> OnApply = new Evt<StatusEffect, StatusEffectReceiver>();
        public Evt<StatusEffect, StatusEffectReceiver> OnRemove = new Evt<StatusEffect, StatusEffectReceiver>();
        private Dictionary<int, StatusEffect> StatusEffectsDictionary;

        private void OnEnable()
        {
            container = new GameObject("StatusEffect Container").transform;
            container.parent = gameObject.transform;
            container.localPosition = Vector3.zero;
        }

        public bool ApplyStatusEffect(StatusEffect effect_, GameObject source_ = null)
        {
            // if stackable and status effect already in effect
            if (effect_.isStackable &&
                IsDuplicateByType(effect_.GetType()))
            {
                effect_.Deactivate();
                return false;
            }

            var _effectInstance = Instantiate(effect_, Vector3.zero, Quaternion.identity, container);

            StatusEffectsDictionary.Add(effect_.gameObject.GetInstanceID(), _effectInstance);

            _effectInstance.Activate(this, source_);
            OnApply?.Invoke(_effectInstance, this);
            return true;
        }

        public void RemoveStatusEffect(int effectID_)
        {
            var _effect = StatusEffectsDictionary[effectID_];
            StatusEffectsDictionary.Remove(effectID_);
            _effect.Deactivate();
            OnRemove?.Invoke(_effect, this);
        }

        public void RemoveStatusEffect(StatusEffect effect_)
        {
            StatusEffectsDictionary.Remove(effect_.gameObject.GetInstanceID());
            effect_.Deactivate();
            OnRemove?.Invoke(effect_, this);
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
}