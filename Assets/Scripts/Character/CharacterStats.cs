using System;

namespace Character
{
    [Serializable]
    public struct CombatStats
    {
        public int maxHp;
        public int maxMana;
        public int weaponDamage;
        public int accuracy;
        public int skillDamage;
        public int armor;
        public int speed;

        public static CombatStats operator +(CombatStats a_, CombatStats b_)
        {
            return new CombatStats
            {
                maxHp = a_.maxHp + b_.maxHp,
                maxMana = a_.maxMana + b_.maxMana,
                weaponDamage = a_.weaponDamage + b_.weaponDamage,
                accuracy = a_.accuracy + b_.accuracy,
                skillDamage = a_.skillDamage + b_.skillDamage,
                armor = a_.armor + b_.armor,
                speed = a_.speed + b_.speed
            };
        }

        public static CombatStats operator -(CombatStats a_, CombatStats b_)
        {
            return new CombatStats
            {
                maxHp = a_.maxHp - b_.maxHp,
                maxMana = a_.maxMana - b_.maxMana,
                weaponDamage = a_.weaponDamage - b_.weaponDamage,
                accuracy = a_.accuracy - b_.accuracy,
                skillDamage = a_.skillDamage - b_.skillDamage,
                armor = a_.armor - b_.armor,
                speed = a_.speed - b_.speed
            };
        }
    }

    public static class CombatStatsHelper
    {
        public static bool IsEmpty(this CombatStats source)
        {
            return source.maxHp == 0 &&
                   source.maxMana == 0 &&
                   source.weaponDamage == 0 &&
                   source.accuracy == 0 &&
                   source.skillDamage == 0 &&
                   source.armor == 0 &&
                   source.speed == 0;
        }
    }
}