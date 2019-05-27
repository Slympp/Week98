using Entities.Interactable;
using Items;
using UI.Player;
using UnityEngine;

namespace Entities.Player {
    public class PlayerTargetingController : MonoBehaviour {

        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Transform _raycastRig;

        private GameObject CurrentTarget;
        private InteractableEntityController _currentTargetController;

        private PlayerEquipmentController _equipmentController;
        private CursorController _cursorController;
        private bool _hasTarget;

        void Awake() {
            _equipmentController = GetComponent<PlayerEquipmentController>();
            _cursorController = GetComponent<CursorController>();
        }
        
        void Update() {

            if (!_equipmentController)
                return;

            BaseItem activeItem = _equipmentController.GetActiveItem();
            if (activeItem) {
                UpdateTarget(activeItem);
            }
        }

        public InteractableEntityController GetTarget() {
            return _hasTarget && CurrentTarget != null && _currentTargetController != null
                ? _currentTargetController
                : null;
        }

        void UpdateTarget(BaseItem activeItem) {

            if (Physics.Raycast(_raycastRig.position, transform.forward, 
                out RaycastHit hit, activeItem.GetRange(), _layerMask)) {

                Debug.DrawLine(_raycastRig.position, hit.point, Color.red);

                GameObject newTarget = hit.collider.gameObject.transform.root.gameObject;
                if (_hasTarget) {
                    if (CurrentTarget != newTarget) {
                        SetTarget(activeItem.GetItemType(), newTarget);
                    }
                } else {
                  SetTarget(activeItem.GetItemType(), newTarget);  
                }
            } else if (_hasTarget) {
               SetTarget(activeItem.GetItemType(), null);
            }
        }

        void SetTarget(BaseItem.Type itemType, GameObject newTarget) {
            _hasTarget = newTarget != null;

            if (CurrentTarget != null) {
                _currentTargetController.ToggleHighlight(false);
            }

            if (_hasTarget) {
                _currentTargetController = newTarget.GetComponent<InteractableEntityController>();
                if (_currentTargetController != null)
                    _currentTargetController.ToggleHighlight(true);
            } else {
                _currentTargetController = null;
            }

            CurrentTarget = newTarget;
            _cursorController.UpdateCursor(itemType, CurrentTarget);
        }
    }
}