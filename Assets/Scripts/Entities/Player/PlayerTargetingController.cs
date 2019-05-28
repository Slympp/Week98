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
        private Camera _camera;
        private bool _hasTarget;

        void Awake() {
            _equipmentController = GetComponent<PlayerEquipmentController>();
            _cursorController = GetComponent<CursorController>();
            _camera = Camera.main;
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

            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, _layerMask)) {
                GameObject newTarget = hit.collider.gameObject.transform.root.gameObject;
                if (_hasTarget) {
                    if (CurrentTarget != newTarget) {
                        SetTarget(newTarget);
                    }
                } else {
                  SetTarget(newTarget);  
                }
            } else if (_hasTarget) {
               SetTarget(null);
            }
            
            UpdateCursorIndicator(activeItem);
        }

        void SetTarget(GameObject newTarget) {
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
        }

        void UpdateCursorIndicator(BaseItem activeItem) {
            float distance = Mathf.Infinity;
            if (CurrentTarget != null) {
                distance = Vector3.Distance(CurrentTarget.transform.position, transform.position);
            }
            _cursorController.UpdateCursor(activeItem.GetItemType(), CurrentTarget && distance <= activeItem.GetRange());
        }
    }
}