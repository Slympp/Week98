using UnityEngine;

namespace Items {
    public class SwordItem : WeaponItem {
        public override void Use(GameObject target) {
            UseWeapon(target);
            
            // play sword animation
        }
    }
}