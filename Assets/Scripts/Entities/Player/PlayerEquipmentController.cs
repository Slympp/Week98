using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Items;
using UI.Player;
using UnityEngine;

namespace Entities.Player {
    public class PlayerEquipmentController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> ItemList;
        [SerializeField] protected Transform RightHandRoot;
        [SerializeField] protected Transform LeftHandRoot;
        [SerializeField] protected Transform BackRoot;

        private BaseItem _activeItem;
        private Dictionary<BaseItem, List<GameObject>> _inventory;
        private CursorController _cursorController;
        private PlayerUIController _uiController;
        
        [SerializeField] protected GameObject ShieldPrefab;
        public bool CanSwapItem = true;

        public bool HasShield { get; private set; }
        public GameObject BackShieldRef { get; private set; }
        public GameObject HandShieldRef { get; private set; }

        void Awake() {
            _uiController = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<PlayerUIController>();
            _cursorController = GetComponent<CursorController>();
            _inventory = new Dictionary<BaseItem, List<GameObject>>();
        }

        void Start() {
            // TODO: REMOVE, TESTING ONLY
            foreach (GameObject item in ItemList) {
                AddItemToInventory(item);
            }
            _uiController.UpdateActionBar(_inventory);
            
            EquipShield();

            // END TESTING
        }
        
        void Update() {

            if (!CanSwapItem)
                return;
            
            if (PlayerInputController.EquipSword) {
                SetActiveItem(typeof(SwordItem));
            } else if (PlayerInputController.EquipTorch) {
                SetActiveItem(typeof(TorchItem));
            } else if (PlayerInputController.EquipBow) {
                SetActiveItem(typeof(BowItem));
            } else if (PlayerInputController.EquipRune) {
                SetActiveItem(typeof(RuneItem));
            } 
        }

        public void AddItemToInventory(GameObject item) {
            BaseItem itemComponent = item.GetComponent<BaseItem>();
            if (!itemComponent)
                return;

            GameObject right = null;
            GameObject left = null;
            List<GameObject> list = new List<GameObject>();
            BaseItem newItem = null;

            switch (itemComponent.GetHandType()) {
                case BaseItem.Hand.Right:
                    right = Instantiate(item, RightHandRoot);
                    newItem = right.GetComponent<BaseItem>();
                    break;
                    
                case BaseItem.Hand.Left:
                    left = Instantiate(item, LeftHandRoot);
                    newItem = left.GetComponent<BaseItem>();
                    break;
                    
                case BaseItem.Hand.Both:
                    right = Instantiate(item, RightHandRoot);
                    left = Instantiate(item, LeftHandRoot);
                    left.SetActive(false);
                    newItem = right.GetComponent<BaseItem>();
                    break;
 
                default:
                    return;
            }
                
            newItem.Init();
            if (right != null) {
                list.Add(right);
            }

            if (left != null) {
                list.Add(left);
            }
                
            _inventory.Add(newItem, list);
        }

        public void EquipShield() {
            HasShield = true;
            HandShieldRef = Instantiate(ShieldPrefab, LeftHandRoot);
            BackShieldRef = Instantiate(ShieldPrefab, BackRoot);
            HandShieldRef.SetActive(false);
        }

        public void ToggleShield(bool equiped) {
            HandShieldRef.SetActive(equiped);
            BackShieldRef.SetActive(!equiped);
        }
        
        private void SetActiveItem(Type type) {

            BaseItem item = GetInInventory(type);

            if (!item) return;
            
            if (_activeItem && item == _activeItem) {
                return;
            }

            if (_activeItem) {
                ToggleActiveGameObjects(false);
            }
                
            _activeItem = item;
            ToggleActiveGameObjects(true);
            _cursorController.UpdateCursor(_activeItem.GetItemType(), false);
            _uiController.SetActiveInActionBar(_activeItem.GetItemType());
        }

        private void ToggleActiveGameObjects(bool enable) {
            foreach (GameObject obj in _inventory[_activeItem]) {
                obj.SetActive(enable);
            }
        }

        private BaseItem GetInInventory(Type type) {
            foreach (var item in _inventory) {
                if (item.Key.GetType() == type)
                    return item.Key;
            }

            return null;
        }
        
        public BaseItem GetActiveItem() {
            return _activeItem;
        }
    }
}
