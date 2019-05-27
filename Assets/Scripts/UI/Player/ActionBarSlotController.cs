using Items;
using UnityEngine;

namespace UI.HUD {
    public class ActionBarSlotController : MonoBehaviour {

        public BaseItem.Type ItemType;
        public GameObject Icon;
        public GameObject Indicator;
        
        [HideInInspector]
        public bool InInventory;

        public void Awake() {
            Icon.SetActive(false);
            Indicator.SetActive(false);
        }
    }
}