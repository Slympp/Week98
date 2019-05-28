using System;
using Entities.Interactable;
using Entities.Interactable.Interactions;
using Entities.Player;
using UnityEngine;

namespace Items {
    public class TorchItem : WeaponItem {

        [SerializeField] private FireEffectProperties Properties;
        
        public override void Use(Transform player, InteractableEntityController target) {
            if (target == null)
                return;
            
            UseWeapon(target);
            
            if (target.InteractionTypes.Contains(InteractableTypes.FLAMMABLE)) {
                Debug.Log($"Set {target.name} on fire");
                target.GetComponent<Firable>().SetOnFire(Properties);
            }
        }

        public override int GetAnimation() {
            return GetRandomAnimation();
        }

        [Serializable]
        public struct FireEffectProperties {
            public int   TickDamage;
            public int   TickAmount;
            public float Duration;
        }
    }
}
