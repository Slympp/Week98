using System.Collections;
using Settings;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD {
    public abstract class BaseBarController : MonoBehaviour {
        [SerializeField] protected Image FillImage;

        private UISettings settings;
        private float      max;
        private float      current;
        private float      Percent => current / max;

        public void Init(UISettings _settings, int _max, int _current) {
            settings = _settings;
            max = _max;
            current = _current;
            FillImage.fillAmount = Percent;
        }

        public void UpdateMaxValue(float _max) {
            if (!max.Equals(_max)) {
                max = _max;
                StartCoroutine(nameof(UpdateBarFillAmount));
            }
        }

        public void UpdateCurrentValue(float _current) {
            if (!current.Equals(_current)) {
                current = _current;
                StartCoroutine(nameof(UpdateBarFillAmount));
            }
        }

        private IEnumerator UpdateBarFillAmount() {
            float originPercent = FillImage.fillAmount;
            float elapsed = 0;

            while (elapsed < settings.BarUpdateSpeed) {
                elapsed += Time.deltaTime;
                FillImage.fillAmount = Mathf.Lerp(originPercent, Percent, elapsed / settings.BarUpdateSpeed);
                yield return new WaitForEndOfFrame();
            }
            FillImage.fillAmount = Percent;
        }
    }
}