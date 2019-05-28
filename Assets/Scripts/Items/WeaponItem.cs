using Entities.Interactable;
using UnityEngine;

namespace Items {
    public abstract class WeaponItem : BaseItem {
        [SerializeField] protected Vector2Int Damage;
        
        protected void UseWeapon(InteractableEntityController target) {
            if (target.InteractionTypes.Contains(InteractableTypes.HITTABLE)) {
                if (gameObject.activeSelf) {
                    StartCoroutine(target.DecreaseHealth(Random.Range(Damage.x, Damage.y + 1)));
                }
            } else {
                Debug.Log("Seems like this entity can't be damaged...");
            }
        }
    }
}