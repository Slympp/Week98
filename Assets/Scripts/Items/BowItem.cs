using Entities.Interactable;
using Entities.Player;
using UnityEngine;

namespace Items {
    public class BowItem : WeaponItem {
        [SerializeField] private float ProjectileSpeed = 10;
        [SerializeField] private GameObject ArrowPrefab;
        
        private Transform t;

        void Awake() {
            t = transform;
        }
        
        public override void Use(Transform player, InteractableEntityController target) {
            GameObject arrow = Instantiate(ArrowPrefab, t.position, t.rotation * Quaternion.Euler(0, -90, 0));
            arrow.GetComponent<BowProjectile>().Init(OnArrowImpact);
            arrow.GetComponent<Rigidbody>().AddForce(arrow.transform.forward * ProjectileSpeed);
        }

        public override int GetAnimation() {
            return GetRandomAnimation();
        }

        public void OnArrowImpact(InteractableEntityController target) {
            UseWeapon(target);
//
//            if (target.InteractionTypes.Contains(InteractableTypes.TOGGLABLE)) {
//                
//            }
//            
//            if (target.InteractionTypes.Contains(InteractableTypes.SWITCHABLE)) {
//                
//            }
        }
    }
}
