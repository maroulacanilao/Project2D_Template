using Items;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.LootMenu
{
    public class UI_LootMenuItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI itemName_TXT;
        [SerializeField] private TextMeshProUGUI num_TXT;
        [SerializeField] private Image typeIcon;

        public Item item { get; private set; }

        public UI_LootMenuItem Initialize(Item item_, Sprite typeSprite_)
        {
            item = item_;
            typeIcon.sprite = typeSprite_;
            itemName_TXT.SetText(item.Data.ItemName);
            switch (item.ItemType)
            {
                case ItemType.Consumable:
                    var consumable = (ItemConsumable) item_; 
                    num_TXT.SetText($"{consumable.StackCount}x");
                    num_TXT.gameObject.SetActive(true);
                    break;
                case ItemType.Gold:
                    var Gold = (ItemGold) item_;
                    num_TXT.SetText($"{Gold.GoldAmount}x");
                    num_TXT.gameObject.SetActive(true);
                    break;
                default:
                    num_TXT.gameObject.SetActive(false);
                    break;
            }
            return this;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            UI_LootMenu.OnShowItemDetail.Invoke(this);
        }
    
        public void OnPointerEnter(PointerEventData eventData)
        {
            UI_LootMenu.OnShowItemDetail.Invoke(this);
        }
    
        public void OnPointerExit(PointerEventData eventData)
        {

        }
    }
}