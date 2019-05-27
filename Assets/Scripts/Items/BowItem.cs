using Entities.Interactable;
using Entities.Player;
using UnityEngine;

namespace Items {
    public class BowItem : BaseItem
    {
        public override void Use(Transform player, InteractableEntityController target) {
            if (target == null)
                return;
            
            // TODO: Fire arrow
        }

        public override int GetAnimation() {
            return GetRandomAnimation();
        }

        public void OnArrowImpact(InteractableEntityController target) {
            if (target.InteractionTypes.Contains(InteractableTypes.HITTABLE)) {
                
            }

            if (target.InteractionTypes.Contains(InteractableTypes.TOGGLABLE)) {
                
            }
            
            if (target.InteractionTypes.Contains(InteractableTypes.SWITCHABLE)) {
                
            }
        }
    }
}
