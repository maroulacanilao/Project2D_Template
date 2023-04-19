using UnityEngine;

public enum DamageType
{
    None = -1,
    Weapon = 0,
    Physical = 1,
    Fire = 2,
    Water = 3,
    Electricity = 4,
    Earth = 5,
    Wind = 6,
}

public struct DamageInfo
{
    public int DamageAmount;
    public GameObject Source;
    public DamageType DamageType;
    public RaycastHit HitInfo;   
    public Vector3 Position;

    public DamageInfo(int damageAmount_, GameObject source_, DamageType damageType_ = DamageType.None, RaycastHit hitInfo_ = default, Vector3 position_ = default)
    {
        DamageAmount = damageAmount_;
        Source = source_;
        DamageType = damageType_;
        HitInfo = hitInfo_;
        Position = position_;
    }
}
