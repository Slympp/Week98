using System.Linq;
using UnityEngine;

namespace Entities {
    
    [RequireComponent(typeof(EntityAnimationController))]
    public abstract class BaseEntity : MonoBehaviour {

        [SerializeField] protected int  maxHealth;

        protected EntityAnimationController _animationController;
        
        protected int currentHealth;
        protected float currentSpeed;
        protected float speedSmoothVelocity;
        
        public bool isDead { get; private set; }

        protected void Awake() {
            Spawn();
            _animationController = GetComponent<EntityAnimationController>();
        }

        protected abstract void Move(Vector2 movementDelta);
        protected abstract void Rotate();

        public void Spawn() {
            currentHealth = maxHealth;
        }

        public void Die() {
            float dieAnimationDelay = 1f;
            Destroy(this, dieAnimationDelay);
            // Play die animation
        }
        
        public void TakeDamage(int value) {
            if (maxHealth > 0 && !isDead) {
                currentHealth -= value;
                if (currentHealth <= 0) {
                    currentHealth = 0;
                    isDead = true;
                    Die();
                }
            }
        }
        
        public void Heal(int value) {
            if (maxHealth > 0 && !isDead) {
                currentHealth += value;
                if (currentHealth > maxHealth) {
                    currentHealth = maxHealth;
                }
            }
        }
    }
}
