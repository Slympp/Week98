using System;
using System.Collections.Generic;
using Entities.Interactable;
using Entities.Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Items {
    public abstract class BaseItem : MonoBehaviour {
        
        [SerializeField] protected string Name;
        [SerializeField] protected float Cooldown = 1f;
        [SerializeField] protected float Range = 1f;
        [SerializeField] protected List<string> UseAnimations;
        [SerializeField] protected Type ItemType;
        [SerializeField] protected PlayerEntityController.ActiveState State;
        [SerializeField] protected Hand HandType;
        [SerializeField] protected bool RequireTarget;
        [SerializeField] protected float UseDelay;
        
        protected List<int> UseAnimationsHash;

        public void Init() {
            gameObject.SetActive(false);
            
            if (UseAnimations != null && UseAnimations.Count != 0) {
                UseAnimationsHash = new List<int>();
                foreach (var anim in UseAnimations) {
                    UseAnimationsHash.Add(Animator.StringToHash(anim));
                }
            }
        }
        
        protected int GetRandomAnimation() {
            if (UseAnimationsHash.Count == 0) return 0;
            
            int randomIndex = Random.Range(0, UseAnimationsHash.Count);
            return UseAnimationsHash[randomIndex];
        }
        
        public abstract void Use(Transform player, InteractableEntityController target);
        public abstract int GetAnimation();

        public string GetName() { return Name; }
        public float GetCooldown() { return Cooldown; }
        public float GetDelay() { return UseDelay; }
        public float GetRange() { return Range; }
        public Type GetItemType() { return ItemType; }
        public PlayerEntityController.ActiveState GetState() { return State; }
        public Hand GetHandType() { return HandType; }
        public bool DoesRequireTarget() { return RequireTarget; }

        public enum Type {
            Sword = 0,
            Torch,
            Bow,
            Rune
        }

        public enum Hand {
            Right = 0,
            Left,
            Both
        }
    }
}