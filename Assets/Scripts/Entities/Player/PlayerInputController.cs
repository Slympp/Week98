using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

namespace Entities.Player {
    public static class PlayerInputController {
        
        public static Vector2 MovementInput => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        public static bool UseActiveItem => Input.GetMouseButtonDown(0);
        public static bool Shielding => Input.GetMouseButton(1);
        public static bool IsRunning => Input.GetKey(KeyCode.LeftShift);
        public static bool ConsumingPotion => Input.GetKey(KeyCode.A);
        public static bool EquipFirstSlot => Input.GetKeyDown(KeyCode.Alpha1);
        public static bool EquipSecondSlot => Input.GetKeyDown(KeyCode.Alpha2);
        public static bool EquipThirdSlot => Input.GetKeyDown(KeyCode.Alpha3);
        public static bool EquipFourthSlot => Input.GetKeyDown(KeyCode.Alpha4);
    }
}