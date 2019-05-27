using System.Collections.Generic;
using Items;
using TMPro;
using UnityEngine;

namespace UI.Player {
    public class PlayerUIController : BaseUIController {

        [SerializeField] private TMP_Text PotionsAmount;
        [SerializeField] private TMP_Text CoinsAmount;

        [SerializeField] private ActionBarController _actionBarController;
    
        public void UpdateActionBar(Dictionary<BaseItem, List<GameObject>> inventory) {
            _actionBarController.Refresh(inventory);
        }

        public void SetActiveInActionBar(BaseItem.Type type) {
            _actionBarController.SetActiveSlot(type);
        }

        public override void UpdatePotionsAmount(int count) {
            PotionsAmount.text = count.ToString();
        }

        public override void UpdateCoinsAmount(int count) {
            CoinsAmount.text = count.ToString();
        }
    }
}
