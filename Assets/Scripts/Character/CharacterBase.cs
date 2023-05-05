using UnityEngine;

namespace Character
{
    [RequireComponent(typeof(DamageReceiver), typeof(StatusEffectReceiver))]
    public class CharacterBase : MonoBehaviour
    {
        [field: SerializeField] public HealthComponent healthComponent { get; private set; }
        [field: SerializeField] public ManaComponent manaComponent { get; private set; }
        [field: SerializeField] public StatusEffectReceiver statusEffectReceiver { get; protected set; }
        [field: SerializeField] public CharacterStatsData statsData { get; private set; }
    }
}