using System.Collections;
using System.Collections.Generic;
using Character;
using CustomHelpers;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/StatsGrowthData", fileName = "StatsGrowthData")]
public class StatsGrowthData : ScriptableObject
{
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
    
    [BoxGroup("FOR EDITOR/DEBUGGING ONLY")]
    public CombatStats maxStats;

    [BoxGroup("FOR EDITOR/DEBUGGING ONLY")]
    public int maxLevel;

    public CombatStats GetLeveledStats(CombatStats statsCap_, int level_, int levelCap_ = 100)
    {
        return new CombatStats
        {
            maxHp = GetLeveledMaxHealth(statsCap_.maxHp,level_,levelCap_),
            maxMana = GetLeveledMaxMana(statsCap_.maxMana,level_,levelCap_),
            weaponDamage = GetLeveledWeaponDamage(statsCap_.weaponDamage,level_,levelCap_),
            accuracy = GetLeveledAccuracy(statsCap_.accuracy,level_,levelCap_),
            skillDamage = GetLeveledSkillDamage(statsCap_.skillDamage,level_,levelCap_),
            armor = GetLeveledArmor(statsCap_.armor,level_,levelCap_),
            speed = GetLeveledSpeed(statsCap_.speed,level_,levelCap_)
        };
    }

    public int GetLeveledMaxHealth(int healthCap_, int currentLevel_, int levelCap_ = 100)
    {
        return healthGrowthCurve.EvaluateScaledCurve(currentLevel_, levelCap_, healthCap_);
    }
    
    public int GetLeveledMaxMana(int manaCap_, int currentLevel_, int levelCap_ = 100)
    {
        return manaGrowthCurve.EvaluateScaledCurve(currentLevel_, levelCap_, manaCap_);
    }
    
    public int GetLeveledWeaponDamage(int weaponDamageCap_, int currentLevel_, int levelCap_ = 100)
    {
        return weaponDamageGrowthCurve.EvaluateScaledCurve(currentLevel_, levelCap_, weaponDamageCap_);
    }
    
    public int GetLeveledAccuracy(int accuracyCap_, int currentLevel_, int levelCap_ = 100)
    {
        return accuracyGrowthCurve.EvaluateScaledCurve(currentLevel_, levelCap_, accuracyCap_);
    }
    
    public int GetLeveledSkillDamage(int skillDamageCap_, int currentLevel_, int levelCap_ = 100)
    {
        return skillDamageGrowthCurve.EvaluateScaledCurve(currentLevel_, levelCap_, skillDamageCap_);
    }
    
    public int GetLeveledArmor(int armorCap_, int currentLevel_, int levelCap_ = 100)
    {
        return armorGrowthCurve.EvaluateScaledCurve(currentLevel_, levelCap_, armorCap_);
    }
    
    public int GetLeveledSpeed(int speedCap_, int currentLevel_, int levelCap_ = 100)
    {
        return speedGrowthCurve.EvaluateScaledCurve(currentLevel_, levelCap_, speedCap_);
    }
}
