using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Players;
using Scripts.Interactions;
using System;
using UnityEngine;

namespace Scripts.Players
{
    public class PlayerInteraction : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private float radius;
        [SerializeField] private LayerMask interactLayer;
        [SerializeField] private Transform center;
        private PlayerInputSO _playerInput;
        public void Initialize(Entity entity)
        {
            _playerInput = (entity as Player).InputSO;
            _playerInput.OnInteractKeyPressed += HandleInteract;
        }
        private void OnDestroy()
        {
            _playerInput.OnInteractKeyPressed -= HandleInteract;
        }

        private void HandleInteract()
        {
            Collider2D target = Physics2D.OverlapCircle(center.position, radius, interactLayer);
            if (target && target.TryGetComponent(out ICanInteract canInteract))
            {
                canInteract.Interact();
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(center.position, radius);
        }
    }
}
