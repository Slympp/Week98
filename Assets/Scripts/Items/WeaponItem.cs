using Entities.Interactable;
using UnityEngine;

namespace Items {
    public abstract class WeaponItem : BaseItem {
        [SerializeField] protected int Damage;
        
        protected void UseWeapon(InteractableEntityController target) {
            if (target.InteractionTypes.Contains(InteractableTypes.HITTABLE)) {
                Debug.Log($"Hit target for {Damage}");
                target.DecreaseHealth(Damage);
            } else {
                Debug.Log("Seems like this entity can't be damaged...");
            }
        }
    }
}