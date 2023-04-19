using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageTypePropertyData", menuName = "ScriptableObjects/Persistent/Damage Type Property Data")]
public class DamageTypePropertyData : ScriptableObject
{
    [System.Serializable]
    public class DamageTypeProperty
    {
        [field: SerializeField] public DamageType DamageType { get; private set; }
        [field: SerializeField] public DamageType Strength { get; private set; }
        [field: SerializeField] public DamageType Weakness { get; private set; }

        [field: SerializeField] public SerializedDictionary<DamageType, float> dmgModifierDictionary { get; private set; }

        public DamageTypeProperty(DamageType damageType_)
        {
            DamageType = damageType_;
            dmgModifierDictionary = new SerializedDictionary<DamageType, float>();
            dmgModifierDictionary.Add(DamageType.Physical,1);
            dmgModifierDictionary.Add(DamageType.Fire,1);
            dmgModifierDictionary.Add(DamageType.Water,1);
            dmgModifierDictionary.Add(DamageType.Electricity,1);
            dmgModifierDictionary.Add(DamageType.Earth,1);
            dmgModifierDictionary.Add(DamageType.Wind,1);
        }
    }

    [SerializeField] private DamageTypeProperty PhysicalDamageProperty = new DamageTypeProperty(DamageType.Physical);
    [SerializeField] private DamageTypeProperty FireDamageProperty = new DamageTypeProperty(DamageType.Fire);
    [SerializeField] private DamageTypeProperty WaterDamageProperty = new DamageTypeProperty(DamageType.Water);
    [SerializeField] private DamageTypeProperty ElectricityDamageProperty = new DamageTypeProperty(DamageType.Physical);
    [SerializeField] private DamageTypeProperty EarthDamageProperty = new DamageTypeProperty(DamageType.Physical);
    [SerializeField] private DamageTypeProperty WindDamageProperty = new DamageTypeProperty(DamageType.Physical);

    public Dictionary<DamageType, DamageTypeProperty> DamageTypePropertyDictionary { get; private set; } = new Dictionary<DamageType, DamageTypeProperty>();

    private void Reset()
    {
        DamageTypePropertyDictionary = new Dictionary<DamageType, DamageTypeProperty>();
        
        DamageTypePropertyDictionary.Add(DamageType.Physical, PhysicalDamageProperty);
        DamageTypePropertyDictionary.Add(DamageType.Fire, FireDamageProperty);
        DamageTypePropertyDictionary.Add(DamageType.Water, WaterDamageProperty);
        DamageTypePropertyDictionary.Add(DamageType.Electricity, ElectricityDamageProperty);
        DamageTypePropertyDictionary.Add(DamageType.Earth, EarthDamageProperty);
        DamageTypePropertyDictionary.Add(DamageType.Wind, WindDamageProperty);
    }
}

public static class DamageTypePropertiesHelper
{
    private static DamageTypePropertyData DamageTypePropertyData;

    private static void InitializeData()
    {
        if (DamageTypePropertyData != null) return;
        // TODO: Set Data
    }
    
    public static float DamageModifierAgainst(this DamageType source, DamageType target)
    {
        InitializeData();
        return 
            DamageTypePropertyData.DamageTypePropertyDictionary[source].dmgModifierDictionary[target];
    }

    public static DamageType Weakness(this DamageType source)
    {
        InitializeData();
        return DamageTypePropertyData.DamageTypePropertyDictionary[source].Weakness;
    }
    
    public static DamageType Strength(this DamageType source)
    {
        InitializeData();
        return DamageTypePropertyData.DamageTypePropertyDictionary[source].Strength;
    }
}
