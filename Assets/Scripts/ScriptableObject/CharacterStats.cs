[System.Serializable]
public class BattleStats
{
    public int maxHp;
    public int maxMana;
    public int weaponDamage;
    public int accuracy;
    public int skillDamage;
    public int armor;
    public int speed;

    public static BattleStats operator +(BattleStats a_, BattleStats b_)
    {
        return new BattleStats
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

    public static BattleStats operator -(BattleStats a_, BattleStats b_)
    {
        return new BattleStats
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

public static class BattleStatsHelper
{
    public static bool IsEmpty(this BattleStats source)
    {
        if (source == null) return true;
        return source.maxHp == 0 &&
               source.maxMana == 0 &&
               source.weaponDamage == 0 &&
               source.accuracy == 0 &&
               source.skillDamage == 0 &&
               source.armor == 0 &&
               source.speed == 0;
    }
}
