using System.Collections;
using Entities.Player;
using Game;
using UI;
using UI.HUD;
using UnityEngine;

namespace Entities {
    
    [RequireComponent(typeof(EntityAnimationController))]
    public abstract class BaseEntity : MonoBehaviour {
        public ActiveState State { get; protected set; }

        [SerializeField] protected int  maxHealth;

        protected int   currentHealth;
        protected float currentSpeed;
        protected float speedSmoothVelocity;
        
        protected GameManager _gameManager;
        protected GameObject _levelManager;
        protected BaseUIController _uiController;
        protected EntityAnimationController _animationController;
        
        protected void Awake() {
            currentHealth = maxHealth;
            
            _gameManager = GameManager.Get();
            _levelManager = GameObject.FindGameObjectWithTag("LevelManager");
            _animationController = GetComponent<EntityAnimationController>();
        }

        protected abstract void Move(Vector2 movementDelta);
        protected abstract void Rotate();
        public abstract IEnumerator TakeDamage(int value);

        public IEnumerator Die() {
            yield return new WaitForSeconds(0.5f);
            float dieAnimationTime = 1f;
            Destroy(gameObject, dieAnimationTime);
            _animationController.PlayDieAnimation();
        }
        
        public IEnumerator DecreaseHealth(int value) {
            if (maxHealth > 0 && State != ActiveState.Dead) {
                
                currentHealth -= value;
                Debug.Log($"{currentHealth}/{maxHealth}");

                if (_uiController)
                    _uiController.UpdateHealthBar(currentHealth);
                
//              _animationController.PlayTakeDamageAnimation();
                
                if (currentHealth <= 0) {
                    Debug.Log("Dead!");
                    currentHealth = 0;
                    State = ActiveState.Dead;
                    yield return Die();
                }
            }
        }
        
        public void Heal(int value) {
            if (maxHealth > 0 && State != ActiveState.Dead) {
                currentHealth += value;
                if (currentHealth > maxHealth) {
                    currentHealth = maxHealth;
                }
            }
        }
        
        public enum ActiveState {
            Default = 0,
            Attacking,
            Shooting,
            Defending,
            Drinking,
            Dead
        }
    }
}
