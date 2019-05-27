using UnityEngine;

namespace Entities.Player {
    public static class PlayerInputController {
        
        public static Vector2 MovementInput => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        public static bool UseActiveItem => Input.GetMouseButton(0);
        public static bool Shielding => Input.GetMouseButton(1);
        public static bool DrinkPotion => Input.GetKeyDown(KeyCode.A);
        public static bool EquipSword => Input.GetKeyDown(KeyCode.Alpha1);
        public static bool EquipTorch => Input.GetKeyDown(KeyCode.Alpha2);
        public static bool EquipBow => Input.GetKeyDown(KeyCode.Alpha3);
        public static bool EquipRune => Input.GetKeyDown(KeyCode.Alpha4);
    }
}