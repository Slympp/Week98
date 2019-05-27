using System.Collections.Generic;
using Entities;
using Items;
using UnityEngine;

namespace Settings {
    
    [CreateAssetMenu(fileName = "Level Settings", menuName = "Settings/Level Settings")]
    public class LevelSettings : ScriptableObject {
        public string Name;
        public bool Procedural;
        public int NumberOfRooms;
        public List<GameObject> Monsters;
        public GameObject Boss;
    }
}