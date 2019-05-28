using UnityEngine;

namespace Game.Effects {
    public class LookAtCamera : MonoBehaviour {
        private Transform _camera;
    
        void Awake() {
            if (Camera.main != null) _camera = Camera.main.transform;
        }

        // Update is called once per frame
        void Update() {
            transform.LookAt(_camera);
        }
    }
}
