using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using Entities.Interactable;
using UnityEngine;

namespace Game.Level {
    public class RoomController : MonoBehaviour {
        public bool North;
        public bool East;
        public bool South;
        public bool West;

        public List<Transform> Doors;
        public List<InteractableEntityController> Monsters;
        public GameObject Fog;
        public bool Completed;

        public void InitDoors() {
            Doors = transform.FindChildren("Door");
            ToggleDoors(false);
        }
        
        private IEnumerator WaitForCompletion() {
            bool complete = false;
            while (!complete) {
                complete = CheckAllMonstersDead();
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

        private void CompleteRoom() {
            Completed = true;
            ToggleDoors(false);
        }
        
        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag("Player") && !Completed) {
                Fog.SetActive(false);
                ToggleDoors(true);
                StartCoroutine(nameof(WaitForCompletion));
            }
        }
        
        private void ToggleDoors(bool active) {
            foreach (Transform door in Doors) {
                door.gameObject.SetActive(active);
            }
        }
    }
}
