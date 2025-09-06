using _00.Work.CDH.Code.Combat;
using _00.Work.CDH.Code.Entities;
using _00.Work.CSH._01.Scripts.Enemies;
using _00.Work.CSH._01.Scripts.Enemies.BTCommons.Actions;
using _00.Work.CSH._01.Scripts.Enemies.BTCommons.Events;
using Unity.Behavior;
using UnityEngine;

namespace _00.Work.CIW._01.Scripts.Enemy
{
    public class Summoner : BTEnemy
    {
        //[SerializeField] StateChangeEvent _stateEvt;

        BlackboardVariable<BTEnemyState> _state;

        EntityMover _mover;

        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            _mover = GetCompo<EntityMover>();
            GetCompo<EntityHealth>().OnKnockBack += HandleKnockback;
        }

        private void HandleKnockback(Vector2 vector)
        {
            float knockBackTime = 0.5f;
            _mover.KnockBack(vector, knockBackTime);
        }

        private void Start()
        {
            BlackboardVariable<StateChangeEvent> stateChannelVariable = GetBlackboardVariable<StateChangeEvent>("StateChannel");
            Debug.Assert(stateChannelVariable != null, $"state channel variable is null in {gameObject.name}"); // error
            Debug.Log(stateChannelVariable == null ? "StateChannelVariable is null" : "StateChannelVariable found");
            _stateChannel = stateChannelVariable.Value;

            _state = GetBlackboardVariable<BTEnemyState>("EnemyState");
            Debug.Assert(_state != null, $"state is null in {gameObject.name}");
        }

        protected override void HandleHit()
        {
            if (isDead) return;

            _state.Value = BTEnemyState.HIT; // 지금 Hit으로 못 가
            _stateChannel.SendEventMessage(BTEnemyState.HIT);
        }

        protected override void HandleDead()
        {
            base.HandleDead();

            _state.Value = BTEnemyState.DEATH;
            _stateChannel.SendEventMessage(BTEnemyState.DEATH);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GetCompo<EntityHealth>().OnKnockBack -= HandleKnockback;
        }
    }
}

