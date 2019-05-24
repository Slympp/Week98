using Items;
using UnityEngine;

namespace Entities.Player {
    public class PlayerTargetingController : MonoBehaviour {

        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Transform _raycastRig;

        public GameObject currentTarget { get; private set; }
        private PlayerEntityController _entityController;

        void Awake() {
            _entityController = GetComponent<PlayerEntityController>();
        }
        
        void Update() {

            if (!_entityController)
                return;

            currentTarget = null;
            BaseItem activeItem = _entityController.GetActiveItem();
            if (activeItem) {
                GetTarget(activeItem);
            }
        }

        void GetTarget(BaseItem activeItem) {

            if (Physics.Raycast(_raycastRig.position, transform.forward, 
                out RaycastHit hit, activeItem.GetRange(), _layerMask)) {

                Debug.DrawLine(_raycastRig.position, hit.point, Color.red);
                currentTarget = hit.collider.gameObject.transform.root.gameObject;
            } else {
            }
        }
    }
}