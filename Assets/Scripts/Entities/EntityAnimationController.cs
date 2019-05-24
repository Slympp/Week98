using Items;
using UnityEngine;

namespace Entities {
    public class EntityAnimationController : MonoBehaviour {
        
        [SerializeField] private float animationSmoothTime = 0.1f;
        
        private int xMovementHash, yMovementHash, movementSpeedHash;
        private Animator _animator;

        void Awake() {
            _animator = GetComponentInChildren<Animator>();

            xMovementHash = Animator.StringToHash("xMovement");
            yMovementHash = Animator.StringToHash("yMovement");
            movementSpeedHash = Animator.StringToHash("movementSpeed");
        }

        public void UpdateMovementAnimation(Vector2 movementDelta, bool isRunning) {
            _animator.SetFloat(xMovementHash, movementDelta.x, animationSmoothTime, Time.deltaTime);
            _animator.SetFloat(yMovementHash, movementDelta.y, animationSmoothTime, Time.deltaTime);
            _animator.SetFloat(movementSpeedHash, Mathf.Clamp((isRunning ? 1 : 0.5f) * movementDelta.magnitude, 0, 1), animationSmoothTime, Time.deltaTime);
        }

        public void PlayAnimation(int animationHash) {
            _animator.Play(animationHash, -1, 0);
        }
    }
}