using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Items;
using TMPro;
using UnityEngine;

namespace UI.Player {
    public class PlayerUIController : BaseUIController {

        [SerializeField] private TMP_Text PotionsAmount;
        [SerializeField] private TMP_Text CoinsAmount;
        
        [SerializeField] private TMP_Text CurrentLevel;
        [SerializeField] private TMP_Text GameTime;

        [SerializeField] private ActionBarController _actionBarController;
        
        private GameManager _gameManager;

        void Awake() {
            _gameManager = GameManager.Get();

            if (_gameManager != null) {
                UpdateLevelName(_gameManager.GetCurrentLevelSettings().Name);
            }
            StartCoroutine(nameof(UpdateGameTime));
        }

        private void OnDisable() {
            StopCoroutine(nameof(UpdateGameTime));
        }
        
        public void UpdateActionBar(Dictionary<BaseItem, List<GameObject>> inventory) {
            _actionBarController.Refresh(inventory);
        }

        public void SetActiveInActionBar(BaseItem.Type type) {
            _actionBarController.SetActiveSlot(type);
        }

        public override void UpdatePotionsAmount(int count) {
            PotionsAmount.text = count.ToString();
        }

        public override void UpdateCoinsAmount(int count) {
            CoinsAmount.text = count.ToString();
        }

        public void UpdateLevelName(string name) {
            CurrentLevel.text = name;
        }

        public IEnumerator UpdateGameTime() {

            while (true) {
                TimeSpan ts = TimeSpan.FromSeconds(_gameManager.GetCurrentTime());

                string formatted = "";
                if (ts.Hours != 0) {
                    formatted += (ts.Hours > 9 ? ts.Hours.ToString() : "0" + ts.Hours) + ":";
                }
                if (ts.Minutes != 0) {
                    formatted += (ts.Hours == 0 ? ts.Minutes.ToString() : ts.Minutes > 9 ? ts.Minutes.ToString() : "0" + ts.Minutes) + ":";
                }
                formatted += ts.Minutes == 0 ? ts.Seconds.ToString() : ts.Seconds > 9 ? ts.Seconds.ToString() : "0" + ts.Seconds;
                GameTime.text = formatted;
                yield return new WaitForSeconds(1);
            }
        }
    }
}
