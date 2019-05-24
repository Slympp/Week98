using UnityEngine;

namespace Settings {
    
    [CreateAssetMenu(fileName = "Player Settings", menuName = "Settings/Player Settings")]
    public class PlayerSettings : ScriptableObject {

        public float RunningMovementSpeed = 1.8f;
        public float MovementSpeed = 1;
        public float MovementSpeedSmoothTime = 0.1f;
        public float UsingItemSpeedReduction = 0.2f;
        
        public float RotationSpeed = 1;
        public float MinRotationDistance = 30f;
        
        public int HealthPotionRegenValue = 1;
    }
}