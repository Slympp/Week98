using Settings;
using UI.HUD;
using UnityEngine;

namespace UI {
    public abstract class BaseUIController : MonoBehaviour {

        [SerializeField] protected UISettings settings;
        [SerializeField] private HealthBarController HealthBarController;

        public void InitHealthBar(int max, int current) {
            HealthBarController.Init(settings, max, current);
        }

        public void UpdateHealthBar(int current) {
            HealthBarController.UpdateCurrentValue(current);
        }

        public abstract void UpdatePotionsAmount(int count);
        public abstract void UpdateCoinsAmount(int count);
    }
}
