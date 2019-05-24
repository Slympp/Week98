using System.Collections.Generic;
using UnityEngine;

namespace Entities.Interactable {
    public class InteractableEntityController : BaseEntity {

        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material highlightedMaterial;

        public List<InteractableTypes> interactionType;
        
        protected override void Move(Vector2 movementDelta) {
            throw new System.NotImplementedException();
        }

        protected override void Rotate() {
            throw new System.NotImplementedException();
        }
    }
}