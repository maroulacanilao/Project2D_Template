using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using CustomEvent;
using CustomHelpers;
using Items;
using NaughtyAttributes;
using UnityEngine;

namespace Character
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Character Stats")]
    public class CharacterStatsData : ScriptableObject
    {
        [field: BoxGroup("Character Stats")]
        [field: SerializeField] public CombatStats baseCombatStats { get; private set; }
        
        [field: BoxGroup("Character Stats")]
        [field: SerializeField] public CombatStats maxLevelCombatStats { get; private set; }

        [field: BoxGroup("Character Stats")]
        [field: SerializeField] [field: SerializedDictionary("DamageType", "Resistance( Range -1 to 1)")]
        public SerializedDictionary<DamageType, float> DamageTypeResistance { get; private set; }
        
        [CurveRange(0, 0, 1, 1, EColor.Yellow)] [BoxGroup("Stats Growth")]
        [SerializeField] private AnimationCurve healthGrowthCurve;
    
        [CurveRange(0, 0, 1, 1, EColor.Yellow)] [BoxGroup("Stats Growth")]
        [SerializeField] private AnimationCurve manaGrowthCurve;
    
        [CurveRange(0, 0, 1, 1, EColor.Yellow)] [BoxGroup("Stats Growth")]
        [SerializeField] private AnimationCurve weaponDamageGrowthCurve;
    
        [CurveRange(0, 0, 1, 1, EColor.Yellow)] [BoxGroup("Stats Growth")]
        [SerializeField] private AnimationCurve accuracyGrowthCurve;
    
        [CurveRange(0, 0, 1, 1, EColor.Yellow)] [BoxGroup("Stats Growth")]
        [SerializeField] private AnimationCurve skillDamageGrowthCurve;
    
        [CurveRange(0, 0, 1, 1, EColor.Yellow)] [BoxGroup("Stats Growth")]
        [SerializeField] private AnimationCurve armorGrowthCurve;
    
        [CurveRange(0, 0, 1, 1, EColor.Yellow)] [BoxGroup("Stats Growth")]
        [SerializeField] private AnimationCurve speedGrowthCurve;

        public readonly Evt<CharacterStatsData> OnTotalStatsUpdate = new Evt<CharacterStatsData>();
        public CombatStats levelStats { get; private set; }
        public CombatStats equipmentStats { get; private set; }
        public CombatStats bonusStats { get; private set; }

        public CombatStats totalStats => baseCombatStats+ levelStats + equipmentStats + bonusStats;

        public List<Item> equipmentStatsSources { get; private set; }
        
        
        private void Reset()
        {
            DamageTypeResistance = new SerializedDictionary<DamageType, float>();
            DamageTypeResistance.Add(DamageType.None, 0);
            DamageTypeResistance.Add(DamageType.Weapon, 0);
            DamageTypeResistance.Add(DamageType.Physical, 0);
            DamageTypeResistance.Add(DamageType.Fire, 0);
            DamageTypeResistance.Add(DamageType.Water, 0);
            DamageTypeResistance.Add(DamageType.Electricity, 0);
            DamageTypeResistance.Add(DamageType.Earth, 0);
            DamageTypeResistance.Add(DamageType.Wind, 0);
        }

        private void OnEnable()
        {
            equipmentStatsSources = new List<Item>();

            equipmentStats = new CombatStats();
            bonusStats = new CombatStats();
        }

        public void SetLevelStats(CombatStats newCombatStats_)
        {
            levelStats = newCombatStats_;
            OnTotalStatsUpdate.Invoke(this);
        }

        public void AddEquipmentStats(CombatStats stats_, Item item_)
        {
            if (!equipmentStatsSources.AddUnique(item_)) return;

            equipmentStats += stats_;

            OnTotalStatsUpdate.Invoke(this);
        }

        public void RemoveEquipmentStats(CombatStats stats_, Item item_)
        {
            if (!equipmentStatsSources.Remove(item_)) return;

            equipmentStats += stats_;

            OnTotalStatsUpdate.Invoke(this);
        }

        /// <summary>
        ///     For consumables or other temporary stats
        /// </summary>
        public void AddBonusStats(CombatStats stats_)
        {
            bonusStats += stats_;
        }

        /// <summary>
        ///     For consumables or other temporary stats
        /// </summary>
        public void RemoveBonusStats(CombatStats stats_)
        {
            bonusStats -= stats_;
        }
        
        #region Stats Growth
        
        public CombatStats GetLeveledStats(int level_, int levelCap_ = 100)
        {
            return new CombatStats
            {
                maxHp = GetLeveledMaxHealth(level_, levelCap_),
                maxMana = GetLeveledMaxMana(level_, levelCap_),
                weaponDamage = GetLeveledWeaponDamage(level_, levelCap_),
                accuracy = GetLeveledAccuracy(level_, levelCap_),
                skillDamage = GetLeveledSkillDamage(level_, levelCap_),
                armor = GetLeveledArmor(level_, levelCap_),
                speed = GetLeveledSpeed(level_, levelCap_)
            };
        }

        public int GetLeveledMaxHealth(int currentLevel_, int levelCap_ = 100)
        {
            var _leveledHp = healthGrowthCurve.EvaluateScaledCurve
                (currentLevel_, levelCap_,maxLevelCombatStats.maxHp - baseCombatStats.maxHp);
            
            return baseCombatStats.maxHp + _leveledHp;
        }

        public int GetLeveledMaxMana(int currentLevel_, int levelCap_ = 100)
        {
            var _leveledMana = manaGrowthCurve.EvaluateScaledCurve
                (currentLevel_, levelCap_, maxLevelCombatStats.maxMana - baseCombatStats.maxMana);
            
            return baseCombatStats.maxMana + _leveledMana;
        }

        public int GetLeveledWeaponDamage(int currentLevel_, int levelCap_ = 100)
        {
            var _leveledWpnDmg =  weaponDamageGrowthCurve.EvaluateScaledCurve
                (currentLevel_, levelCap_, maxLevelCombatStats.weaponDamage - baseCombatStats.weaponDamage);
            
            return baseCombatStats.weaponDamage + _leveledWpnDmg;
        }

        public int GetLeveledAccuracy(int currentLevel_, int levelCap_ = 100)
        {
            var _leveledAcc = accuracyGrowthCurve.EvaluateScaledCurve
                (currentLevel_, levelCap_, maxLevelCombatStats.accuracy - baseCombatStats.accuracy);
            
            return baseCombatStats.accuracy + _leveledAcc;
        }

        public int GetLeveledSkillDamage(int currentLevel_, int levelCap_ = 100)
        {
            var _leveledSklDmg = skillDamageGrowthCurve.EvaluateScaledCurve
                (currentLevel_, levelCap_, maxLevelCombatStats.skillDamage - baseCombatStats.skillDamage);
            
            return baseCombatStats.skillDamage + _leveledSklDmg;
        }

        public int GetLeveledArmor(int currentLevel_, int levelCap_ = 100)
        {
            var _leveledArm = armorGrowthCurve.EvaluateScaledCurve
                (currentLevel_, levelCap_, maxLevelCombatStats.armor - baseCombatStats.armor);
            
            return baseCombatStats.armor + _leveledArm;
        }

        public int GetLeveledSpeed(int currentLevel_, int levelCap_ = 100)
        {
            var _leveledSpeed = speedGrowthCurve.EvaluateScaledCurve
                (currentLevel_, levelCap_, maxLevelCombatStats.speed - baseCombatStats.speed);
            
            return baseCombatStats.armor + _leveledSpeed;
        }
        
        #endregion
    }
}