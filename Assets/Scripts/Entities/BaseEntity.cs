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
        
        protected GameObject _gameManager;
        protected GameObject _levelManager;
        protected BaseUIController _uiController;
        protected EntityAnimationController _animationController;
        
        protected void Awake() {
            Spawn();
            
            _gameManager = GameManager.Get().gameObject;
            _levelManager = GameObject.FindGameObjectWithTag("LevelManager");
            _animationController = GetComponent<EntityAnimationController>();
        }

        protected abstract void Move(Vector2 movementDelta);
        protected abstract void Rotate();
        public abstract IEnumerator TakeDamage(int value);

        public void Spawn() {
            currentHealth = maxHealth;
        }

        public IEnumerator Die() {
            yield return new WaitForSeconds(0.5f);
            float dieAnimationTime = 1f;
            Destroy(this, dieAnimationTime);
            _animationController.PlayDieAnimation();
        }
        
        public IEnumerator DecreaseHealth(int value) {
            if (maxHealth > 0 && State != ActiveState.Dead) {
                currentHealth -= value;
                if (_uiController)
                    _uiController.UpdateHealthBar(currentHealth);
                _animationController.PlayTakeDamageAnimation();
                if (currentHealth <= 0) {
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
