﻿using UnityEngine;

namespace Game.Level {
    public class ExitDoor : MonoBehaviour {
        private GameManager _gameManager;
        
        void Awake() {
            _gameManager = GameManager.Get();
        }
        
        void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag("Player")) {
                _gameManager.LoadNextLevel();
            }
        }
    }
}
