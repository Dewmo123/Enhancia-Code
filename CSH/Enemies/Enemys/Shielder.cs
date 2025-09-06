using _00.Work.CDH.Code.Entities;
using _00.Work.CSH._01.Scripts.Enemies.BTCommons.Actions;
using _00.Work.CSH._01.Scripts.Enemies.BTCommons.Events;
using Unity.Behavior;
using UnityEngine;

namespace _00.Work.CSH._01.Scripts.Enemies
{

    public class Sheider : BTEnemy
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
            //float knockBackTime = 0.5f;
            //_mover.KnockBack(Vector2.zero, knockBackTime);

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


           
        }

        protected override void HandleDead()
        {
            _stateChannel.SendEventMessage(BTEnemyState.DEATH);
        }


    }
}