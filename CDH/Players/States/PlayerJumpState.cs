using _00.Work.CDH.Code.Animators;
using _00.Work.CDH.Code.Entities;
using UnityEngine;

namespace _00.Work.CDH.Code.Players.States
{
    public class PlayerJumpState : PlayerAirState
    {
        public PlayerJumpState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _player.OnJump?.Invoke();
            _mover.StopImmediately(true); // 공중에서 떨어지는 힘을 리셋하고 점프 가능

            --_player.JumpCount;
            _mover.Jump();

            _mover.OnVelocity.AddListener(HandleVelocityChange);
        }

        public override void Exit()
        {
            _mover.OnVelocity.RemoveListener(HandleVelocityChange);
            base.Exit();
        }

        private void HandleVelocityChange(Vector2 velocity)
        {
            if (velocity.y < 0)
            {
                _player.ChangeState("FALL");
            }
        }
    }
}