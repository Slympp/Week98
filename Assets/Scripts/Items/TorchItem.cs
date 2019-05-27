using Entities.Interactable;
using Entities.Player;
using UnityEngine;

namespace Items {
    public class TorchItem : WeaponItem {
        
        public override void Use(Transform player, InteractableEntityController target) {
            if (target == null)
                return;
            
            UseWeapon(target);
            
            if (target.InteractionTypes.Contains(InteractableTypes.FLAMMABLE)) {
                // TODO: Apply fire effect
                Debug.Log($"Set {target.name} on fire");
            }
        }

        public override int GetAnimation() {
            return GetRandomAnimation();
        }
    }
}
