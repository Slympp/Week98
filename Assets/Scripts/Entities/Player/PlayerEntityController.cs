using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Entities.Interactable;
using Game;
using Items;
using Settings;
using UI;
using UI.Player;
using UnityEngine;

namespace Entities.Player {
    
    [RequireComponent(typeof(CharacterController))]
    public class PlayerEntityController : BaseEntity {

        [SerializeField] private PlayerSettings PlayerSettings;
        
        private CharacterController _characterController;
        private PlayerTargetingController _targetingController;
        private PlayerEquipmentController _equipmentController;
        private CursorController _cursorController;
        private Camera _camera;

        private int _healthPotionsCount;
        private int _coinsCount;
        private float _remainingCooldown;
        private bool _usingItem;

        void Awake() {

            base.Awake();
            
            _camera = Camera.main;
            _characterController = GetComponent<CharacterController>();
            _targetingController = GetComponent<PlayerTargetingController>();
            _equipmentController = GetComponent<PlayerEquipmentController>();
            _cursorController = GetComponent<CursorController>();
        }

        void Start() {
            Init(_gameManager.PlayerData);
            
            if (_levelManager) {
                _uiController = _levelManager.GetComponent<PlayerUIController>();
                _uiController.InitHealthBar(maxHealth, currentHealth);
                _uiController.UpdatePotionsAmount(_healthPotionsCount);
                _uiController.UpdateCoinsAmount(_coinsCount);
            }
        }
        
        void Update() {

            if (_camera == null && (_camera = Camera.main) == null) {
                return;
            }

            if (State == ActiveState.Dead) {
                _equipmentController.CanSwapItem = false;
                return;
            }

            if (State != ActiveState.Defending) {
                if (State != ActiveState.Shooting && State != ActiveState.Drinking) {
                    Move(PlayerInputController.MovementInput);
                }
                Rotate();
            }

            if (Defend())
                return;
            
            if (PlayerInputController.DrinkPotion) {
                ConsumePotion();
            } else if (PlayerInputController.UseActiveItem && _remainingCooldown <= 0
                && State == ActiveState.Default) {
                UseActiveItem(); 
            }
        }

        public void Init(GameManager.SerializedPlayerData playerData) {
            currentHealth = playerData.CurrentHealth;
            _healthPotionsCount = playerData.HealthPotionsCount;
            _equipmentController.Init(playerData);
        }

        public GameManager.SerializedPlayerData GetSerializedPlayerData() {
            return new GameManager.SerializedPlayerData {
                CurrentHealth =  currentHealth,
                HealthPotionsCount = _healthPotionsCount,
                CoinsCount = _coinsCount,
                Inventory = _equipmentController.ItemList,
                HasShield = _equipmentController.HasShield
            };
        }

        protected override void Move(Vector2 movementDelta) {
            
            if (!_characterController)
                return;

            Vector3 moveDirection = new Vector3(movementDelta.x, 0, movementDelta.y);

            float targetSpeed = PlayerSettings.MovementSpeed 
                                * GetSpeedModifier()
                                * movementDelta.magnitude;

            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, PlayerSettings.MovementSpeedSmoothTime * Time.deltaTime);

            moveDirection *= currentSpeed;
            
            Vector3 localVelocity = transform.InverseTransformDirection(_characterController.velocity);
            
            _animationController.UpdateMovementAnimation(localVelocity);
            _characterController.Move(Vector3.ClampMagnitude(moveDirection, currentSpeed) * Time.deltaTime);
        }

        protected override void Rotate() {

            Vector3 entityPosition = transform.position;
            Vector3 position = _camera.WorldToScreenPoint(entityPosition);
            Vector3 direction = Input.mousePosition - position;

            if (Vector3.Distance(direction, entityPosition) <= PlayerSettings.MinRotationDistance)
                return;
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            float rotationSpeed = PlayerSettings.RotationSpeed * GetSpeedModifier();
            
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.AngleAxis(angle, Vector3.down), 
                Time.deltaTime * rotationSpeed);
        }

        public override IEnumerator TakeDamage(int value) {
            yield return DecreaseHealth(value);

            if (State == ActiveState.Dead) {
                GameManager.Get().LoadGameOver();
            }
        }
        
        private float GetSpeedModifier() {
            return State != ActiveState.Default ? PlayerSettings.UsingItemSpeedReduction : 1;
        }

        private void UseActiveItem() {
            
            BaseItem activeItem = _equipmentController.GetActiveItem();
            if (activeItem != null && _targetingController != null) {

                if (activeItem.DoesRequireTarget() && _targetingController.GetTarget() == null)
                    return;

                bool hasDelay = !activeItem.GetCooldown().Equals(0);
                _animationController.PlayAnimation(activeItem.GetAnimation(), !hasDelay);
                StartCoroutine(nameof(StartCooldown), activeItem);
                State = activeItem.GetState();

                InteractableEntityController interactableTarget = null;
                if (_targetingController.GetTarget() != null) {
                    interactableTarget = _targetingController.GetTarget();
                    if (activeItem.DoesRequireTarget() && interactableTarget == null)
                        return;
                }

                if (hasDelay) {
                    StartCoroutine(nameof(DelayUse), new DelayUseParams {
                        item = activeItem,
                        target = interactableTarget
                    });
                } else {
                    activeItem.Use(transform, interactableTarget);
                }
            }

            // RESET PUSH/PULL ACTION
//            if (_state == ActiveState.VerticalMovementLocked) {
//                _state = ActiveState.Default;
//            }
        }
        
        private bool Defend() {

            if (!_equipmentController.HasShield)
                return false;

            BaseItem activeItem = _equipmentController.GetActiveItem();
            
            bool isShielding = PlayerInputController.Shielding
                               && (State != ActiveState.Shooting || State != ActiveState.Attacking)
                               && activeItem != null 
                               && (activeItem.GetItemType() == BaseItem.Type.Sword || 
                                   activeItem.GetItemType() == BaseItem.Type.Torch);

            if (isShielding && State != ActiveState.Defending) {
                State = ActiveState.Defending;
                _equipmentController.ToggleShield(true);
                _cursorController.UpdateCursor(activeItem.GetItemType(), false, true);
            } else if (!isShielding && State == ActiveState.Defending) {
                State = ActiveState.Default;
                _equipmentController.ToggleShield(false);
                _cursorController.UpdateCursor(activeItem.GetItemType(), false);
            }

            _animationController.UpdateDefense(isShielding);

            return isShielding;
        }

        private void ConsumePotion() {
            if (_healthPotionsCount > 0) {
                _healthPotionsCount--;

                State = ActiveState.Drinking;
                _animationController.PlayDrinkAnimation();
                StartCoroutine(nameof(PotionHealing));
            }
        }

        private IEnumerator PotionHealing() {
            float elapsed = 0;
            while (elapsed < PlayerSettings.DrinkPotionDelay) {
                elapsed += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            Heal(PlayerSettings.HealthPotionRegenValue);
            _uiController.UpdatePotionsAmount(_healthPotionsCount);
            State = ActiveState.Default;
        }

        private IEnumerator StartCooldown(BaseItem item) {
            _remainingCooldown = item.GetCooldown();
            while (_remainingCooldown > 0) {
                _remainingCooldown -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            _remainingCooldown = 0;
            State = ActiveState.Default;
        }

        private IEnumerator DelayUse(DelayUseParams p) {
            float elapsed = 0;
            while (elapsed <= p.item.GetDelay()) {
                elapsed += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            p.item.Use(transform, p.target);
        }

        struct DelayUseParams {
            public BaseItem item;
            public InteractableEntityController target;
        }
    }
}