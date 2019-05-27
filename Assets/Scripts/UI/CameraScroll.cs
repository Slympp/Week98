using Cinemachine;
using UnityEngine;

namespace UI {
    public class CameraScroll : MonoBehaviour {

        [SerializeField] private float ScrollSensitivity = 1;
        [SerializeField] private Vector2 FovClamp;

        private CinemachineFramingTransposer vcam;
        
        void Awake() {
            vcam = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        }
        
        void Update() {
            float fov = vcam.m_CameraDistance;
            fov -= Input.GetAxis("Mouse ScrollWheel") * ScrollSensitivity;
            fov = Mathf.Clamp(fov, FovClamp.x, FovClamp.y);
            vcam.m_CameraDistance = fov;
        }
    }
}
