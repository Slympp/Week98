using UnityEngine;

namespace Entities {
    public class EntityAnimationController : MonoBehaviour {
        
        [SerializeField] private float animationSmoothTime = 0.1f;
        
        private int xMovementHash, yMovementHash, defendingHash;
        private int DrinkPotionHash, TakeDamageAnimationHash, DieAnimationHash;
        
        private Animator _animator;

        void Awake() {
            _animator = GetComponentInChildren<Animator>();

            xMovementHash = Animator.StringToHash("xMovement");
            yMovementHash = Animator.StringToHash("yMovement");
            defendingHash = Animator.StringToHash("defend");
            
            DrinkPotionHash = Animator.StringToHash("DrinkPotion");
            TakeDamageAnimationHash = Animator.StringToHash("TakeDamage");
            DieAnimationHash = Animator.StringToHash("Die");
        }

        public void UpdateMovementAnimation(Vector3 movementVelocity) {
            _animator.SetFloat(xMovementHash, movementVelocity.x, animationSmoothTime, Time.deltaTime);
            _animator.SetFloat(yMovementHash, movementVelocity.z, animationSmoothTime, Time.deltaTime);
        }

        public void PlayAnimation(int animationHash, bool forcePlay = true) {
            if (animationHash == 0)
                return;
            
            if (forcePlay) {
                _animator.Play(animationHash, -1, 0);
            } else {
                _animator.Play(animationHash);
            }
        }

        public void PlayTakeDamageAnimation() {
            _animator.Play(TakeDamageAnimationHash);
        }
        
        public void PlayDieAnimation() {
            _animator.Play(DieAnimationHash);
        }
        
        public void PlayDrinkAnimation() {
            _animator.Play(DrinkPotionHash);
        }
        
        public void UpdateDefense(bool enable) {
            _animator.SetBool(defendingHash, enable);
        }
    }
}