using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

namespace Items {
    public abstract class BaseItem : MonoBehaviour {
        
        [SerializeField] protected string Name;
        [SerializeField] protected float Cooldown = 1f;
        [SerializeField] protected float Range = 1f;
        [SerializeField] protected bool RightHand = true;

        [SerializeField] protected List<string> UseAnimations;
        [SerializeField] List<int> UseAnimationsHash;

        void Awake() {
            if (UseAnimations != null && UseAnimations.Count > 0) {
                Debug.Log("Init animations");
                UseAnimationsHash = new List<int>();
                foreach (var anim in UseAnimations) {
                    UseAnimationsHash.Add(Animator.StringToHash(anim));
                    Debug.Log(anim);
                }
                Debug.Log($"{UseAnimationsHash.Count} inited");
            }
        }
        
        public abstract void Use(GameObject target);

        public string GetName() { return Name; }
        public float GetCooldown() { return Cooldown; }
        public float GetRange() { return Range; }
        public bool IsRightHand() { return RightHand; }

        public int GetRandomAnimation() {
            Debug.Log($"{UseAnimations.Count} anims set");
            Debug.Log($"{UseAnimationsHash.Count} animsHash set");
//            if (UseAnimationsHash.Count == 0) return 0;
            return 0;
//            int randomIndex = Random.Range(0, UseAnimationsHash.Count);
//            return UseAnimationsHash[randomIndex];
        }
    }
}