using _00.Work.CDH.Code.Animators;
using _00.Work.CDH.Code.Combat;
using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Entities.FSM;
using UnityEngine;

namespace _00.Work.CDH.Code.Players.States
{
    public class PlayerJumpAttackState : PlayerAirState
    {
        public PlayerJumpAttackState(Entity entity, AnimParamSO animParam) : base(entity, animParam) { }

        public override void Enter()
        {
            base.Enter();
            _mover.StopImmediately(true);
            _mover.SetGarvityScale(0.1f);
            _mover.CanManualMove = false;

            SetAttackData();
        }

        private void SetAttackData()
        {
            AttackDataSO attackData = _attackCompo.GetAttackData("player_jump_attack");
            Vector2 movement = attackData.movement;
            movement.x *= _renderer.FacingDirection;
            _mover.AddForceToEntity(movement);
            
            _attackCompo.SetAttackData(attackData);
        }

        public override void Update()
        {
            base.Update();
            if (_isTriggerCall)
            {
                _player.ChangeState("FALL");
            }
        }

        public override void Exit()
        {
            _mover.CanManualMove = true;
            _mover.SetGarvityScale(1f);
            base.Exit();
        }
    }
}