using Items.Inventory;
using UnityEngine;

namespace Character
{
    public class PlayerCharacter : CharacterBase
    {
        [field: SerializeField] public InventoryData inventory { get; private set; }

        private void Awake()
        {
            inventory.Initialize(this);
        }
    }
}