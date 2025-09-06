using _00.Work.CDH.Code.Animators;
using _00.Work.CDH.Code.Entities;
using UnityEngine;

namespace _00.Work.CDH.Code.Players.States
{
    public class PlayerWallSlideState : PlayerGroundState
    {
        private const float WALL_SLIDE_GRAVITY_SCALE = 0.3f;

        private bool _beforeWallSildeDirection; // true = Right

        public PlayerWallSlideState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
        }

        public override void Enter()
        {
            // _onGround때문에
            _renderer.SetParam(_animParam, true);
            _isTriggerCall = false;
            // ---EntityState

            _player.InputSO.OnJumpKeyPressed += HandleJumpKeyPressed;
            _player.InputSO.OnAttackKeyPressed += HandleAttackKeyPressed;
            // ---EntityGroundState

            _mover.StopImmediately(true);
            _mover.SetGarvityScale(WALL_SLIDE_GRAVITY_SCALE);

            if (_onGround || _beforeWallSildeDirection != _renderer.FacingDirection > 0.5f)
            {
                ++_player.JumpCount;
            }
            _beforeWallSildeDirection = _renderer.FacingDirection > 0.5f;

            _onGround = false;
        }

        public override void Update()
        {
            base.Update();
            float xInput = _player.InputSO.InputDirection.x;
            if (Mathf.Abs(xInput + _renderer.FacingDirection) < 0.5f)
            {
                _player.ChangeState("FALL");
                return;
            }
            // 쭉 내려가다가 땅에 닿으면 idle로 변경
            if (_mover.IsGroundDetected() || _mover.IsWallDetected(_renderer.FacingDirection) == false)
            {
                _player.ChangeState("IDLE");
            }
        }

        public override void Exit()
        {
            _mover.SetGarvityScale(1f);

            base.Exit();
        }
    }
}