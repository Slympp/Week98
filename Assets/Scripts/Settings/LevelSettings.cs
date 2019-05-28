using System.Collections.Generic;
using Cinemachine;
using Entities;
using Entities.Player;
using Items;
using UnityEngine;
using UnityEngine.UI;

namespace Settings {
    
    [CreateAssetMenu(fileName = "Level Settings", menuName = "Settings/Level Settings")]
    public class LevelSettings : ScriptableObject {
        public string Name;
        public int NumberOfRooms;
        public Vector2Int MonstersPerRoom;
        public List<GameObject> Templates;
        public List<GameObject> Monsters;
        public GameObject Boss;
        
        [SerializeField] private GameObject LevelReward;
        [SerializeField] private Texture2D RewardIcon;
        [SerializeField] private string RewardDescription;
        public virtual void AddReward(PlayerEquipmentController equipmentController) {
            equipmentController.ItemList.Add(LevelReward);
        }
    }
}