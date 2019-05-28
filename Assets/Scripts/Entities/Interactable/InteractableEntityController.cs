using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

namespace Entities.Interactable {
    public class InteractableEntityController : BaseEntity {

        [SerializeField] private EntityBehaviour Behaviour;
        [SerializeField] private float MinAttackRange = 1;
        [SerializeField] private float MaxAttackRange = 1;
        [SerializeField] private float FleeRange = 3f;
        [SerializeField] private float FleeDelay = 1f;
        
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material highlightedMaterial;

        public List<InteractableTypes> InteractionTypes;

        private MeshRenderer _meshRenderer;
        private NavMeshAgent _navMeshAgent;

        private Transform _player;
        
        void Awake() {
            base.Awake();
            
            _meshRenderer = GetComponent<MeshRenderer>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            ToggleHighlight(false);
        }

        void Start() {
            _player = GameObject.FindWithTag("Player").transform;
        }

        void Update() {

            if (Behaviour == EntityBehaviour.STATIC)
                return;
            
            GetInRange();
        }

        void GetInRange() {
            float distance = Vector3.Distance(_player.position, transform.position);
            if (distance > MaxAttackRange) {
                _navMeshAgent.isStopped = false;
                _navMeshAgent.SetDestination(_player.position);
            } else {
                _navMeshAgent.isStopped = true;
                Vector3 position = _player.position;
                position.y = transform.position.y;
                transform.LookAt(position);
            }
        }

        public void ToggleHighlight(bool highlighted) {
            _meshRenderer.material = highlighted ? highlightedMaterial : defaultMaterial;
        }
        
        protected override void Move(Vector2 position) {
            _navMeshAgent.SetDestination(new Vector3(position.x, 0, position.y));
        }

        protected override void Rotate() {
            throw new System.NotImplementedException();
        }

        public override IEnumerator TakeDamage(int value) {
            yield return DecreaseHealth(value);
        }

        public enum EntityBehaviour {
            MELEE,
            RANGED,
            STATIC
        }
    }
}