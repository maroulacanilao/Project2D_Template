using NaughtyAttributes;
using UnityEngine;

namespace Items.ItemData
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ItemData/ArmorData", fileName = "ArmorData")]
    public class ArmorData : ItemData
    {
        [field: MinMaxSlider(0, 100)] [field: BoxGroup("Armor")]
        [field: SerializeField] public Vector2Int PossibleArmorValue { get; private set; }
        
        [field: CurveRange(0, 0, 1, 1, EColor.Yellow)] [field: BoxGroup("Armor")]
        [field: SerializeField] public AnimationCurve ValueProbabilityCurve { get; private set; }

        private void Reset()
        {
            itemType = ItemType.Armor;
        }

        private void OnValidate()
        {
            itemType = ItemType.Armor;
        }
        
        public int GetRandomArmorValue()
        {
            var _range = PossibleArmorValue.y - PossibleArmorValue.x;
            var _rng = UnityEngine.Random.value;
            var _value = ValueProbabilityCurve.Evaluate(_rng) * (float) _range;
            return Mathf.RoundToInt(_value + PossibleArmorValue.x);
        }
    }
}