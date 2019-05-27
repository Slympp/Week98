using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entities.Interactable {
    public class InteractableEntityController : BaseEntity {

        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material highlightedMaterial;

        public List<InteractableTypes> InteractionTypes;

        private MeshRenderer _meshRenderer;
        
        void Awake() {
            _meshRenderer = GetComponent<MeshRenderer>();
            ToggleHighlight(false);
        }
        
        public void ToggleHighlight(bool highlighted) {
            _meshRenderer.material = highlighted ? highlightedMaterial : defaultMaterial;
        }
        
        protected override void Move(Vector2 movementDelta) {
            throw new System.NotImplementedException();
        }

        protected override void Rotate() {
            throw new System.NotImplementedException();
        }

        public override IEnumerator TakeDamage(int value) {
            yield return DecreaseHealth(value);
        }
    }
}