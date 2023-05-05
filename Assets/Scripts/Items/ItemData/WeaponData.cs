using NaughtyAttributes;
using UnityEngine;

namespace Items.ItemData
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ItemData/WeaponData", fileName = "WeaponData")]
    public class WeaponData : ItemData
    {
        [field: MinMaxSlider(0, 100)] [field: BoxGroup("Weapon")]
        [field: SerializeField] public Vector2Int PossibleWeaponDamage { get; private set; }
        
        [field: CurveRange(0, 0, 1, 1, EColor.Yellow)] [field: BoxGroup("Weapon")]
        [field: SerializeField] public AnimationCurve ValueProbabilityCurve { get; private set; }

        private void Reset()
        {
            itemType = ItemType.Weapon;
        }

        private void OnValidate()
        {
            itemType = ItemType.Weapon;
        }
        
        public int GetRandomWeaponDamage()
        {
            var _range = PossibleWeaponDamage.y - PossibleWeaponDamage.x;
            var _rng = UnityEngine.Random.value;
            var _value = ValueProbabilityCurve.Evaluate(_rng) * (float) _range;
            return Mathf.RoundToInt(_value + PossibleWeaponDamage.x);
        }
    }
}