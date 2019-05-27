using Entities.Interactable;
using Entities.Player;
using UnityEngine;

namespace Items {
    
    public class SwordItem : WeaponItem {
       
        public override void Use(Transform player, InteractableEntityController target) {
            if (target == null)
                return;
            
            UseWeapon(target);
        }

        public override int GetAnimation() {
            return GetRandomAnimation();
        }
    }
}