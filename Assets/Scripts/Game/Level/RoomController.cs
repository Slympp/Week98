using System.Collections;
using System.Collections.Generic;
using Entities;
using Entities.Interactable;
using Entities.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Level {
    public class RoomController : MonoBehaviour {
        public bool North;
        public bool East;
        public bool South;
        public bool West;

        public bool Completed { get; private set; }
        public bool ExitRoom;

        public List<Transform> Doors;
        public List<InteractableEntityController> Monsters;
        public GameObject Fog;
        public GameObject Exit;
        
        private NavMeshSurface NavMesh;
        
        private readonly string _exitPath = "Prefabs/Level/Exit";

        void Awake() {
            NavMesh = GetComponent<NavMeshSurface>();
        }

        public void Init() {
            Doors = transform.FindChildren("Door");
            ToggleDoors(false);

            if (ExitRoom) {
                Exit = (GameObject)Instantiate(Resources.Load(_exitPath), transform);
                Exit.SetActive(false);
            }
            
            NavMesh.BuildNavMesh();
        }
        
        private IEnumerator WaitForCompletion() {
            bool complete = false;
            while (!complete) {
                if (!(complete = CheckAllMonstersDead()))
                    yield return new WaitForSeconds(1);
            }
            CompleteRoom();
        }

        private bool CheckAllMonstersDead() {
            if (Monsters == null || Monsters.Count == 0)
                return true;
            
            foreach (var monster in Monsters) {
                if (monster.State != BaseEntity.ActiveState.Dead)
                    return false;
            }
            return true;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag("Player") && !Completed) {
                StartRoom();
            }
        }
        
        

        private void StartRoom() {
            Fog.SetActive(false);
            ToggleDoors(true);
            ToggleMonsters(true);
            StartCoroutine(nameof(WaitForCompletion));
        }
        
        private void CompleteRoom() {
            Completed = true;
            ToggleMonsters(false);
            ToggleDoors(false);

            if (ExitRoom) {
                GameObject player = GameObject.FindWithTag("Player");
                if (player != null) {
                    PlayerEquipmentController equipmentController = player.GetComponent<PlayerEquipmentController>();
                    GameManager.Get().GetCurrentLevelSettings().AddReward(equipmentController);
                    equipmentController.RefreshInventory();
                    
                    Debug.Log("Yay, you found a new fancy item !");
                }
                Exit.SetActive(true);
            }
        }
        
        private void ToggleDoors(bool active) {
            foreach (Transform door in Doors) {
                door.gameObject.SetActive(active);
            }
        }
        
        private void ToggleMonsters(bool active) {
            foreach (var monster in Monsters) {
                if (monster != null) {
                    monster.gameObject.SetActive(active);
                }
            }
        }
    }
}
