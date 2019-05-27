using Entities.Interactable;
using UnityEngine;

namespace Items {
    public class RuneItem : BaseItem {

        private CharacterController _playerController;
        private Vector3 velocity;
        
        public override void Use(Transform player, InteractableEntityController target) {
            
            if (target == null)
                return;

            if (player != null) {
                _playerController = player.GetComponent<CharacterController>();
            }

            if (_playerController == null)
                return;
            
            velocity = player.InverseTransformDirection(_playerController.velocity);
            if (velocity.z >= 0.1) {
                
            } else if (velocity.z <= -0.1) {
                
            } else {
                
            }
        }

        public override int GetAnimation() {
            if (velocity.z >= 0.1) {             // PUSH
                return UseAnimationsHash[0];
            }
            if (velocity.z <= -0.1) {            // PULL
                return UseAnimationsHash[1];
            }

            return 0;
        }
    }
}
