using _00.Work.CDH.Code.Entities;
using _00.Work.CSH._01.Scripts.Enemies.BTCommons.Actions;
using _00.Work.CSH._01.Scripts.Enemies.BTCommons.Events;
using DewmoLib.ObjectPool.RunTime;
using Unity.Behavior;
using UnityEngine;

namespace _00.Work.CSH._01.Scripts.Enemies
{
    
    public class EnemyDefault : BTEnemy
    {
        private BlackboardVariable<BTEnemyState> _state;

        private EntityMover _mover;
        private EntityFeedbackData _feedbackData;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            _mover = GetCompo<EntityMover>();
            _feedbackData = GetCompo<EntityFeedbackData>();
            GetCompo<EntityHealth>().OnKnockBack += HandleKnockback;
        }

        private void HandleKnockback(Vector2 knockbackForce)
        {
            float knockBackTime = 0.5f;
            _mover.KnockBack(knockbackForce, knockBackTime);

        }

        private void Start()
        {
            BlackboardVariable<StateChangeEvent> stateChannelVariable = GetBlackboardVariable<StateChangeEvent>("StateChannel");
            _stateChannel = stateChannelVariable.Value; 
            _state = GetBlackboardVariable<BTEnemyState>("EnemyState");
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GetCompo<EntityHealth>().OnKnockBack -= HandleKnockback;

        }


        protected override void HandleHit()
        {
            if (isDead) return;

            if (_state.Value == BTEnemyState.STUN || _state.Value == BTEnemyState.HIT) return;

            if (_feedbackData.IsLastHitPowerAttack)
            {
                _stateChannel.SendEventMessage(BTEnemyState.HIT);

            }
            else if (_state.Value == BTEnemyState.PATROL)
            {
                _stateChannel.SendEventMessage(BTEnemyState.CHASE);

            }
        }

        protected override void HandleDead()
        {
            base.HandleDead();
            _stateChannel.SendEventMessage(BTEnemyState.DEATH);
        }

        
    }
}