using System.Collections.Generic;
using Items;
using UI.HUD;
using UnityEngine;

namespace UI.Player {
    public class ActionBarController : MonoBehaviour {
        [SerializeField] private List<ActionBarSlotController> slots;
    
        public void SetActiveSlot(BaseItem.Type activeItemType) {
            foreach (ActionBarSlotController slot in slots) {
                slot.Indicator.SetActive(slot.ItemType == activeItemType && slot.InInventory);
            }
        }
    
        public void Refresh(Dictionary<BaseItem, List<GameObject>> inventory) {
            foreach (ActionBarSlotController slot in slots) {
                slot.Icon.SetActive(slot.InInventory = ContainsItemOftype(inventory, slot.ItemType));
            }
        }

        private bool ContainsItemOftype(Dictionary<BaseItem, List<GameObject>> inventory, BaseItem.Type type) {
            foreach (BaseItem item in inventory.Keys) {
                if (item.GetItemType() == type)
                    return true;
            }
            return false;
        }
    }
}
