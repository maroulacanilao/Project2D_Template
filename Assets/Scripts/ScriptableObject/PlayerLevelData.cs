using CustomHelpers;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Level Data", fileName = "Player Level Data")]
public class PlayerLevelData : ScriptableObject
{
    [field: CurveRange(0, 0, 1, 1, EColor.Red)]
    [field: SerializeField] public AnimationCurve expLvlCurve { get; private set; }
    [field: SerializeField] public int LevelCap { get; private set; } = 100;
    [field: SerializeField] public int ExperienceCap { get; private set; } = 10000;

    private int currLvl;
    private int totalExperience;

    public int CurrentLevel => currLvl;
    public int TotalExperience => totalExperience;
    public int NextLevelExperience => EvaluateExperience(currLvl + 1);
    public int PrevLevelExperience => currLvl <= 0 ? 0 : EvaluateExperience(currLvl - 1);
    public int CurrentLevelExperience => totalExperience - PrevLevelExperience;
    public int ExperienceNeededToLevelUp => NextLevelExperience - totalExperience;

    public void AddExp(int expAmount_)
    {
        totalExperience += expAmount_;
        totalExperience = Mathf.Clamp(totalExperience, 0, ExperienceCap);

        currLvl = EvaluateExperience(currLvl);
        if (currLvl > LevelCap) currLvl = LevelCap;
    }

    public int EvaluateExperience(int level_)
    {
        return expLvlCurve.EvaluateScaledCurve(level_, LevelCap, ExperienceCap);
    }
}
