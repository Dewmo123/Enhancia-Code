using _00.Work.CDH.Code.Animators;
using _00.Work.CDH.Code.Entities;
using UnityEngine;

namespace _00.Work.CDH.Code.Players.States
{
    public class PlayerMoveState : PlayerGroundState
    {
        private float _stateTime;
        public PlayerMoveState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _stateTime = Time.time;
            _player.InputSO.OnSlideKeyPressed += HandleSlideKey;
        }

        public override void Update()
        {
            base.Update();
            float xInput = _player.InputSO.InputDirection.x;

            _mover.SetMovementX(xInput);

            if (Mathf.Approximately(xInput, 0) || _mover.IsWallDetected(_renderer.FacingDirection)) // 유사 근접 체크, 
            {
                _player.ChangeState("IDLE");
            }
        }

        public override void Exit()
        {
            _player.InputSO.OnSlideKeyPressed -= HandleSlideKey;
            base.Exit();
        }

        private void HandleSlideKey()
        {
            float overSlideTime = 0.3f;
            if (_stateTime + overSlideTime < Time.time)
                _player.ChangeState("SLIDE");
        }

        protected override void HandleAttackKeyPressed()
        {
            float overDashTime = 0.3f;
            if (_stateTime + overDashTime < Time.time)
                _player.ChangeState("DASH_ATTACK");
            else
                base.HandleAttackKeyPressed();
        }
    }
}
