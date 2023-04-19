using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using CustomEvent;
using CustomHelpers;
using UnityEngine;

namespace Stats
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Character Stats")]
    public class CharacterStatsData : ScriptableObject
    {
        [field: SerializeField] public BattleStats baseBattleStats { get; private set; }
        public BattleStats levelStats { get; private set; }
        public BattleStats equipmentStats { get; private set; }
        public BattleStats bonusStats { get; private set; }
        
        public BattleStats totalStats => levelStats + equipmentStats + bonusStats;

        [field: SerializeField] [field: SerializedDictionary("DamageType","Resistance( Range -1 to 1)")]
        public SerializedDictionary<DamageType, float> DamageTypeResistance { get; private set; }

        public List<IEquippable> equipmentStatsSources { get; private set; }

        public readonly Evt<CharacterStatsData> OnTotalStatsUpdate = new Evt<CharacterStatsData>();
        
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
            equipmentStatsSources = new List<IEquippable>();
            
            equipmentStats = new BattleStats();
            bonusStats = new BattleStats();
        }

        public void SetLevelStats(BattleStats newBattleStats_)
        {
            levelStats = newBattleStats_;
            OnTotalStatsUpdate.Invoke(this);
        }
        
        public void AddEquipmentStats(IEquippable equippable_)
        {
            if(!equipmentStatsSources.AddUnique(equippable_)) return;
            equipmentStats += equippable_.equipmentStats;
            OnTotalStatsUpdate.Invoke(this);
        }
        
        public void RemoveEquipmentStats(IEquippable equippable_)
        {
            if(!equipmentStatsSources.Remove(equippable_)) return;
            equipmentStats += equippable_.equipmentStats;
            OnTotalStatsUpdate.Invoke(this);
            OnTotalStatsUpdate.Invoke(this);
        }
        
        /// <summary>
        /// For consumables or other temporary stats
        /// </summary>
        public void AddBonusStats(IConsumable consumable_)
        {
            
        }
        
        
    }
}