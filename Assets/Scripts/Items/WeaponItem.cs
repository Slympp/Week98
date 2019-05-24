using Entities.Interactable;
using UnityEngine;

namespace Items {
    public abstract class WeaponItem : BaseItem {
        [SerializeField] protected int Damage;
        
        protected void UseWeapon(GameObject target) {
            if (!target)
                return;

            InteractableEntityController entity = target.GetComponent<InteractableEntityController>();
            if (entity) {
                if (entity.interactionType.Contains(InteractableTypes.HITTABLE)) {
                    Debug.Log($"Hit target for {Damage}");
                    entity.TakeDamage(Damage);
                } else {
                    Debug.Log("Seems like this entity can't be damaged...");
                }
            }
        }
    }
}