using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Player;
using Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game {
    public class GameManager : MonoBehaviour {

        public SerializedPlayerData PlayerData { get; private set; }
        public int CurrentLevel { get; private set; } = -1;

        [SerializeField] private List<LevelSettings> Levels;

        [SerializeField] private SerializedPlayerData DefaultPlayerData;
        
        private static GameManager _instance;

        private float _time;
        
        void Awake() {
            if (_instance != null) {
                DestroyImmediate(this);
                return;
            }

            PlayerData = DefaultPlayerData;

            _instance = this;
            DontDestroyOnLoad(_instance.gameObject);
        }

        private IEnumerator GameChrono() {
            while (true) {
                _time++;
                yield return new WaitForSeconds(1);
            }
        }

        public static GameManager Get() {
            return _instance;
        }
        
        public void LoadNextLevel() {

            if (_time.Equals(0)) {
                StartCoroutine(nameof(GameChrono));
            }

            GameObject player = GameObject.FindWithTag("Player");
            if (player != null) {
                PlayerData = player.GetComponent<PlayerEntityController>().GetSerializedPlayerData();
            } else {
                PlayerData = DefaultPlayerData;
            }
            
            CurrentLevel++;
            SceneManager.LoadScene("Game");
        }

        public LevelSettings  GetCurrentLevelSettings() {
            if (CurrentLevel == -1)
                CurrentLevel++;
            
            if (CurrentLevel < Levels.Count) {
                return Levels[CurrentLevel];
            }
            return null;
        }

        public float GetCurrentTime() {
            return _time;
        }

        public void LoadGameOver() {
            SceneManager.LoadScene("GameOver");
        }

        public void LoadMainMenu() {
            SceneManager.LoadScene("MainMenu");
        }

        [Serializable]
        public struct SerializedPlayerData {
            public int CurrentHealth;
            public int HealthPotionsCount;
            public int CoinsCount;
            public List<GameObject> Inventory;
            public bool HasShield;
        }
    }
}
