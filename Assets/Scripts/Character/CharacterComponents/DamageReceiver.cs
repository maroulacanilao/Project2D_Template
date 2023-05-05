using System;
using CustomHelpers;
using UnityEngine;

namespace Character
{
    public enum AttackResult
    {
        Miss,
        Block,
        Hit,
        Critical
    }

    public class DamageReceiver : MonoBehaviour
    {
        private HealthComponent healthComponent;
        private CharacterBase owner;
        private CharacterStatsData statsData;

        private void Awake()
        {
            owner = GetComponentInParent<CharacterBase>();
            statsData = owner.statsData;
            healthComponent = owner.healthComponent;
        }

        public AttackResult TakeDamage(DamageInfo damageInfo_)
        {
            //if (IsEvaded(damageInfo_.Source)) return AttackResult.Miss;

            var res = statsData.DamageTypeResistance[damageInfo_.DamageType];
            var dmgTypeMult = 1 - res;
            var armorDmgMult = GetArmorDamageMultiplier();

            var finalDmg = damageInfo_.DamageAmount * dmgTypeMult * armorDmgMult;
            damageInfo_.DamageAmount = Mathf.RoundToInt(finalDmg);
            healthComponent.TakeDamage(damageInfo_);
            return AttackResult.Hit;
        }

        private float GetArmorDamageMultiplier()
        {
            return 1 - 0.052f * statsData.totalStats.armor
                / (0.9f + 0.048f * Math.Abs(statsData.totalStats.armor));
        }

        private bool IsEvaded(GameObject attacker)
        {
            if (!attacker.TryGetComponent(out CharacterBase _characterBase)) return false;

            var attackerAcc = _characterBase.statsData.totalStats.speed + _characterBase.statsData.totalStats.accuracy;
            var totalSum = 1 + attackerAcc * 2 + statsData.totalStats.speed;
            return GeneralHelper.RandomBool(statsData.totalStats.speed / totalSum);
        }
    }
}