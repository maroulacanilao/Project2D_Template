using System;
using CustomEvent;
using ObjectPool;
using UI.LootMenu;
using UnityEngine;

namespace Items
{
    public class LootDropObject : MonoBehaviour, IInteractable, IPoolable
    {
        // [SerializeField] private Outline outline;
        [SerializeField] private Material outLineMaterial;
        public LootDrop lootDrop { get; private set; }

        private Material defaultMaterial;
        private new Renderer renderer;

        public static readonly Evt<LootDropObject> OnLootInteract = new Evt<LootDropObject>();

        private void Awake()
        {
            renderer = GetComponent<Renderer>();
            defaultMaterial = renderer.material;
        }
        
        public LootDropObject Initialize(LootDrop lootDrop_)
        {
            lootDrop = lootDrop_;
            // outline.enabled = false;
            
            return this;
        }
        
        public void OnSpawn()
        {
            
        }
        public void OnDeSpawn()
        {
            lootDrop = default;
        }

        public void OnInteract()
        {
            OnLootInteract.Invoke(this);
        }

        public void OnEnter()
        {
            // renderer.material = outLineMaterial;
            // outline.enabled = true;
        }

        public void OnExit()
        {
            // renderer.material = defaultMaterial;
            // outline.enabled = false;
        }
    }
}