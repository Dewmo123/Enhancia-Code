using _00.Work.CDH.Code.Animators;
using _00.Work.CDH.Code.Entities;
using System;
using UnityEngine;

namespace _00.Work.CDH.Code.Players.States
{
    public class PlayerIdleState : PlayerGroundState
    {
        public PlayerIdleState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _mover.StopImmediately(false);
        }

        public override void Update()
        {
            base.Update();

            float xInput = _player.InputSO.InputDirection.x;

            float facingDirection = _renderer.FacingDirection;
            if (Mathf.Abs(facingDirection + xInput) > 1.5f && _mover.IsWallDetected(facingDirection)) return;

            if (MathF.Abs(xInput) > 0)
            {
                _player.ChangeState("MOVE");
            }
        }
    }
}
