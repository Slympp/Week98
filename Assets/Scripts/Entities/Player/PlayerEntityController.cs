using System.Collections;
using System.Collections.Generic;
using Items;
using Settings;
using UnityEngine;

namespace Entities.Player {
    
    [RequireComponent(typeof(CharacterController))]
    public class PlayerEntityController : BaseEntity {

        [SerializeField] private PlayerSettings PlayerSettings;

        [SerializeField] private BaseItem ActiveItem;
        [SerializeField] private List<GameObject> ItemList;

        [SerializeField] protected Transform RightHandRoot;
        [SerializeField] protected Transform LeftHandRoot;

        private Dictionary<BaseItem, GameObject> _instantiatedItems;
        
        private CharacterController _characterController;
        private PlayerTargetingController _targetingController;
        private Camera _camera;

        private int _healthPotionsCount;
        private float _remainingCooldown;
        private bool _usingItem;


        void Awake() {
            base.Awake();
            
            InitializeItems();
            
            _camera = Camera.main;
            _characterController = GetComponent<CharacterController>();
            _targetingController = GetComponent<PlayerTargetingController>();
        }
        
        void Update() {

            Move(PlayerInputController.MovementInput);

            Rotate();

            if (PlayerInputController.EquipFirstSlot) {
                SwapActiveItem(0);
            } else if (PlayerInputController.EquipSecondSlot) {
                SwapActiveItem(1);
            } else if (PlayerInputController.EquipThirdSlot) {
                SwapActiveItem(2);
            } else if (PlayerInputController.EquipFourthSlot) {
                SwapActiveItem(3);
            } else if (PlayerInputController.UseActiveItem && _remainingCooldown <= 0 ) {
                UseActiveItem(); 
            } else if (PlayerInputController.Shielding) {
                Shield();
            } else if (PlayerInputController.ConsumingPotion) {
                ConsumePotion();
            } else {
                // TODO: reset consuming potion progression
            }
        }

        public BaseItem GetActiveItem() {
            return ActiveItem;
        }

        private void InitializeItems() {
            _instantiatedItems = new Dictionary<BaseItem, GameObject>();
            foreach (GameObject item in ItemList) {
                InstantiateNewItem(item);
            }
        }

        private void InstantiateNewItem(GameObject gameObject) {
            if (!gameObject)
                return;

            BaseItem item = gameObject.GetComponent<BaseItem>();
            if (!item)
                return;
            
            GameObject newItem = Instantiate(item.gameObject, item.IsRightHand() ? RightHandRoot : LeftHandRoot);
            newItem.SetActive(false);
            _instantiatedItems.Add(item, newItem);
        }

        private GameObject GetInstantiatedItem(BaseItem item) {
            if (_instantiatedItems.ContainsKey(item)) {
                return _instantiatedItems[item];
            }
            return null;
        }

        protected override void Move(Vector2 movementDelta) {
            
            if (!_characterController)
                return;
            
            
            
            Vector3 moveDirection = transform.TransformDirection(new Vector3(movementDelta.x, 0, movementDelta.y));

            bool isMovingForward = movementDelta.y >= 0.8 && Mathf.Abs(movementDelta.x) <= 0.5f;
            bool isRunning = PlayerInputController.IsRunning && isMovingForward;
            float targetSpeed = (isRunning ? PlayerSettings.RunningMovementSpeed : 
                                    PlayerSettings.MovementSpeed) 
                                * (isMovingForward ? 1 : 0.5f)
                                * (_usingItem ? PlayerSettings.UsingItemSpeedReduction : 1)
                                * movementDelta.magnitude;

            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity,
                PlayerSettings.MovementSpeedSmoothTime);
            
            _animationController.UpdateMovementAnimation(movementDelta, isRunning);
            
            moveDirection *= currentSpeed;

            _characterController.Move(moveDirection * Time.deltaTime);
        }

        protected override void Rotate() {

            Vector3 entityPosition = transform.position;
            
            var pos = _camera.WorldToScreenPoint(entityPosition);
            var dir = Input.mousePosition - pos;

            if (Vector3.Distance(dir, entityPosition) <= PlayerSettings.MinRotationDistance)
                return;
            
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            float rotationSpeed = PlayerSettings.RotationSpeed * (_usingItem ? PlayerSettings.UsingItemSpeedReduction : 1);
            
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.AngleAxis(angle, Vector3.down), 
                Time.deltaTime * rotationSpeed);
        }

        private void UseActiveItem() {
            if (ActiveItem != null && _targetingController != null && _targetingController.currentTarget != null) {

                _usingItem = true;
                _animationController.PlayAnimation(ActiveItem.GetRandomAnimation());
                ActiveItem.Use(_targetingController.currentTarget);
                StartCoroutine(nameof(StartCooldown), ActiveItem);
            }
        }

        private IEnumerator StartCooldown(BaseItem item) {
            _remainingCooldown = item.GetCooldown();
            while (_remainingCooldown > 0) {
                _remainingCooldown -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            _remainingCooldown = 0;
            _usingItem = false;
        }

        private void SwapActiveItem(int slot) {
            if (slot < 0 || slot >= ItemList.Count || ItemList[slot] == null) {
                Debug.Log($"Failed to equip item {slot}");
                return;
            }

            BaseItem itemComponent = ItemList[slot].GetComponent<BaseItem>();

            if (ActiveItem != null && itemComponent == ActiveItem) {
                Debug.Log($"{ItemList[slot]} is already active");
                return;
            }

            if (ActiveItem != null) {
                GetInstantiatedItem(ActiveItem)?.SetActive(false);
            }
                
            ActiveItem = itemComponent;
            GetInstantiatedItem(ActiveItem)?.SetActive(true);
            Debug.Log($"{ActiveItem} has been equiped");
        }

        private void Shield() {
            
        }

        private void ConsumePotion() {
            // TODO: make consuming progress system
            
            if (_healthPotionsCount > 0) {
                Heal(PlayerSettings.HealthPotionRegenValue);
                _healthPotionsCount--;
            }
        }
    }
}