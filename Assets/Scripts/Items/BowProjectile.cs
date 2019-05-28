using System;
using Entities.Interactable;
using UnityEngine;

namespace Items {
    public class BowProjectile : MonoBehaviour {
        private Action<InteractableEntityController> _onImpact;
        
        public void Init(Action<InteractableEntityController> onImpact) {
            _onImpact = onImpact;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag("Interactable")) {
                InteractableEntityController target = other.GetComponent<InteractableEntityController>();
                if (target != null)
                    _onImpact(target);
            } 
            
            if (!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("NotInteractable")) {
                Destroy(gameObject);
            }
        }
    }
}
