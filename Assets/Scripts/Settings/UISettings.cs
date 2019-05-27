using UnityEngine;

namespace Settings {
    [CreateAssetMenu(fileName = "UI Settings", menuName = "Settings/UI", order = 1)]
    public class UISettings : ScriptableObject {
        public float BarUpdateSpeed = 0.2f;
    }
}