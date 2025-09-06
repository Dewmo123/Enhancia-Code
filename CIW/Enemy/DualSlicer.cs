using _00.Work.CDH.Code.Entities;
using _00.Work.CSH._01.Scripts.Enemies;
using _00.Work.CSH._01.Scripts.Enemies.BTCommons.Actions;
using _00.Work.CSH._01.Scripts.Enemies.BTCommons.Events;
using Unity.Behavior;
using UnityEngine;

namespace _00.Work.CIW._01.Scripts.Enemy
{
    public class DualSlicer : BTEnemy
    {
        //[SerializeField] StateChangeEvent _stateEvt;
        //[SerializeField] PoolItemSO _poolItem;
        //[SerializeField] EnemyAttackCompo _attackCompo;

        //BlackboardVariable<float> _attackRange;
        //BlackboardVariable<float> _detectRange;

        BlackboardVariable<BTEnemyState> _state;

        EntityMover _mover;

        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            _mover = GetCompo<EntityMover>();
            GetCompo<EntityHealth>().OnKnockBack += HandleKnockback;

            //_attackCompo = GetCompo<EnemyAttackCompo>();
            //_poolManager.Pop(PoolItem);
        }

        private void HandleKnockback(Vector2 vector)
        {
            float knockBackTime = 0.5f;
            _mover.KnockBack(vector, knockBackTime);
        }

        private void Start()
        {
            BlackboardVariable<StateChangeEvent> stateChannelVariable = GetBlackboardVariable<StateChangeEvent>("StateChannel");
            Debug.Log(stateChannelVariable);
            Debug.Assert(stateChannelVariable != null, $"state channel variable is null {gameObject.name}");

            _stateChannel = stateChannelVariable.Value;
            Debug.Assert(_stateChannel != null, $"Dual Slicer StateChannel variable is null {gameObject.name}");

            _state = GetBlackboardVariable<BTEnemyState>("EnemyState");
            Debug.Assert(_state != null, $"EnemyState variable is null {gameObject.name}");

            //_attackRange = GetBlackboardVariable<float>("AttackRange");
            //_detectRange = GetBlackboardVariable<float>("DetectRange");

            //_detectRange.Value = _attackCompo.detectDistance;
            //_attackRange.Value = _attackCompo.attackDistance;
        }

        protected override void HandleHit()
        {
            if (isDead) return;

            _state.Value = BTEnemyState.HIT; // 이게 실행이 안 됨. 오류 검출
            _stateChannel.SendEventMessage(BTEnemyState.HIT);
        }

        protected override void HandleDead()
        {
            base.HandleDead();
            _state.Value = BTEnemyState.DEATH; // 얘도
            _stateChannel.SendEventMessage(BTEnemyState.DEATH);

            // BT가 끝날 때까지 대기하다가 실행되었으면 좋겠음
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GetCompo<EntityHealth>().OnKnockBack -= HandleKnockback;
        }
    }
}

