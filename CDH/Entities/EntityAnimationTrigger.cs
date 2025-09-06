using System;
using _00.Work.CDH.Code.Entities;
using Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.Events;

namespace _00.Work.CDH.Code.Entities
{
    public class EntityAnimationTrigger : MonoBehaviour, IEntityComponent
    {
        public event Action OnAnimationEnd;
        public event Action OnAttackTrigger;
        public event Action OnSkillTrigger;
        public event Action OnJumpTrigger;
        public UnityEvent OnWalkTrigger;
        private Entity _entity;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        private void AnimationEnd() => OnAnimationEnd?.Invoke();
        private void AttackTrigger() => OnAttackTrigger?.Invoke();

        private void JumpStart() => OnJumpTrigger?.Invoke();
        private void SkillTrigger() => OnSkillTrigger?.Invoke();
        private void WalkTrigger() => OnWalkTrigger?.Invoke();
    }
}