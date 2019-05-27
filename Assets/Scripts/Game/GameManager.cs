using System.Collections.Generic;
using Settings;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game {
    public class GameManager : MonoBehaviour {
        
        public int CurrentLevel { get; private set; }

        [SerializeField] private int                 StartLevel;
        [SerializeField] private List<LevelSettings> Levels;
        
        private static GameManager _instance;
        
        void Awake() {
            if (_instance != null) {
                DestroyImmediate(this);
            }

            _instance = this;
            CurrentLevel = StartLevel;
            DontDestroyOnLoad(gameObject);
        }

        public static GameManager Get() {
            return _instance;
        }
        
        public void LoadNextLevel() {
            CurrentLevel++;
            SceneManager.LoadScene("Game");
        }

        public LevelSettings  GetCurrentLevelSettings() {
            if (CurrentLevel < Levels.Count) {
                return Levels[CurrentLevel];
            }

            Debug.Log("Load EndGame screen");
            return null;
        }

        public void LoadGameOver() {
            
        }

        public void LoadMainMenu() {
            
        }
        
        public void Load() {
            // TODO: unserialized game data
        }

        public void Save() {
            // TODO: serialize game data
            // - currentLevel
            // - currentHealth
            // time
            // coins
        }
    }
}
