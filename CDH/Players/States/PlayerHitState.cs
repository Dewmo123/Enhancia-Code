using _00.Work.CDH.Code.Animators;
using _00.Work.CDH.Code.Entities;
using UnityEngine;

namespace _00.Work.CDH.Code.Players.States
{
    public class PlayerHitState : PlayerAirState
    {
        public PlayerHitState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _mover.CanManualMove = false;
            _mover.StopImmediately(true);
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