using System.Collections;
using Items;
using UnityEngine;

namespace Entities.Interactable.Interactions {
    public class Firable : MonoBehaviour {
        private InteractableEntityController _entityController;

        private readonly string FirePrefabPath = "Prefabs/Effects/FireEffect";
        private GameObject FireEffectRef;
        
        void Awake() {
            _entityController = GetComponent<InteractableEntityController>();
            FireEffectRef = Instantiate((GameObject) Resources.Load(FirePrefabPath), transform);
            FireEffectRef.SetActive(false);
        }

        public void SetOnFire(TorchItem.FireEffectProperties props) {
            
            StopCoroutine(nameof(ApplyFire));
            StartCoroutine(nameof(ApplyFire), props);
        }

        private IEnumerator ApplyFire(TorchItem.FireEffectProperties props) {
            FireEffectRef.SetActive(true);
            float elapsedTime = 0;
            int currentTick = 0;
            while (currentTick < props.TickAmount) {
                if (elapsedTime < props.Duration / props.TickAmount) {
                    elapsedTime += Time.deltaTime;
                } else {
                    elapsedTime = 0;
                    currentTick++;
                    StartCoroutine(_entityController.TakeDamage(props.TickDamage));
                }
                yield return new WaitForEndOfFrame();
            }
            FireEffectRef.SetActive(false);
        }
    }
}
