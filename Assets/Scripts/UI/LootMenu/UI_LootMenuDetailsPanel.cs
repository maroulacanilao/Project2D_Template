using System;
using System.Collections;
using CustomHelpers;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LootMenu
{
    public class UI_LootMenuDetailsPanel : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject namePanel, statsPanel, descriptionPanel;

        [Header("Text")]
        [SerializeField] private TextMeshProUGUI nameTxt, typeTxt, valueTxt, statsTxt, descriptionTxt;

        [Header("Buttons")]
        [SerializeField] private Button lootBtn, trashBtn;

        [Header("Image")]
        [SerializeField] private Image itemIcon;
        
        private UI_LootMenuItem currLootMenuItem;

        public void Initialize(UI_LootMenu lootMenu_)
        {
            lootBtn.onClick.AddListener((() => lootMenu_.Loot(currLootMenuItem)));
            trashBtn.onClick.AddListener((() => lootMenu_.RemoveMenuItem(currLootMenuItem)));
        }

        private void OnDisable()
        {
            currLootMenuItem = null;
        }

        public void ShowItemDetail(UI_LootMenuItem lootMenuItem_)
        {
            if (lootMenuItem_ == null || lootMenuItem_.item == null)
            {
                gameObject.SetActive(false);
                return;
            }
            
            currLootMenuItem = lootMenuItem_;
            var _currItem = lootMenuItem_.item;
            var _data = _currItem.Data;
        
            namePanel.SetActive(_currItem.ItemType != ItemType.Gold);
            statsPanel.SetActive(_currItem.ItemType != ItemType.Gold);
            descriptionPanel.SetActive(_currItem.ItemType != ItemType.Gold);

            if (_currItem.ItemType == ItemType.Gold)
            {
                var gold = (ItemGold) _currItem;
                valueTxt.SetText($"Value: {gold.GoldAmount}");
                return;
            }
        
            valueTxt.SetText($"Value: {_data.ItemBaseValue}");
            nameTxt.SetText(_data.ItemName);
            typeTxt.SetText(_data.ItemType.ToString());
            descriptionTxt.SetText(_data.Description);
            itemIcon.sprite = _data.Icon;

            switch (_data.ItemType)
            {
                case ItemType.Armor:
                {
                    var _armorItem = (ItemArmor) _currItem;
                    statsTxt.SetText($"Armor: {_armorItem.Stats.armor}");
                    break;
                }

                case ItemType.Weapon:
                {
                    var _weaponItem = (ItemWeapon) _currItem;
                    statsTxt.SetText($"Armor: {_weaponItem.Stats.weaponDamage}");
                    break;
                }

                default:
                    statsPanel.SetActive(false);
                    break;
            }
            gameObject.SetActive(true);
        }
    }
}
