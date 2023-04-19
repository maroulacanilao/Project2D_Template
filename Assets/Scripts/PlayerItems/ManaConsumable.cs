using Stats;
using UnityEngine;

public class ManaConsumable : MonoBehaviour, IConsumable
{
    [field: SerializeField] public BattleStats bonusStats { get; set; }
    [field: SerializeField] public ItemType ItemType { get; set; }
    [field: SerializeField] public int healAmount { get; private set; }
    
    public void OnConsume(CharacterStatsData characterStatsData_)
    {
        
    }
}
