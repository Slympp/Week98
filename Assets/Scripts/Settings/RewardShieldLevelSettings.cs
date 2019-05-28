using Entities.Player;
using UnityEngine;

namespace Settings {
    
    [CreateAssetMenu(fileName = "Level Settings", menuName = "Settings/Level Settings (Reward Shield)")]
    public class RewardShieldLevelSettings : LevelSettings {

        public override void AddReward(PlayerEquipmentController equipmentController) {
            if (equipmentController) {
                equipmentController.AddShield();
            }
        }
    }
}
