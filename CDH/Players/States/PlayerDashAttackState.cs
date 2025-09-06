using _00.Work.CDH.Code.Animators;
using _00.Work.CDH.Code.Combat;
using _00.Work.CDH.Code.Entities;
using UnityEngine;

namespace _00.Work.CDH.Code.Players.States
{
    public class PlayerDashAttackState : PlayerGroundState
    {
        private PlayerAttackCompo _playerAttackCompo;

        public PlayerDashAttackState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _playerAttackCompo = _player.GetCompo<PlayerAttackCompo>();
        }

        public override void Enter()
        {
            base.Enter();
            _mover.CanManualMove = false;

            SetAttackData();
        }

        private void SetAttackData()
        {
            AttackDataSO attackDataSO = _playerAttackCompo.GetAttackData("player_dash_attack");
            Vector2 movement = attackDataSO.movement;
            movement.x *= _renderer.FacingDirection;
            _mover.AddForceToEntity(movement);
            
            _playerAttackCompo.SetAttackData(attackDataSO);
        }

        public override void Update()
        {
            base.Update();
            if (_isTriggerCall)
                _player.ChangeState("IDLE");
        }

        public override void Exit()
        {
            _mover.CanManualMove = true;
            base.Exit();
        }
    }
}