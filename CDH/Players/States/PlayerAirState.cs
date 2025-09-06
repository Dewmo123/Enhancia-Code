using _00.Work.CDH.Code.Animators;
using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Entities.FSM;
using UnityEngine;

namespace _00.Work.CDH.Code.Players.States
{
    public abstract class PlayerAirState : EntityState
    {
        protected Player _player;
        protected EntityMover _mover;
        protected PlayerAttackCompo _attackCompo;

        private const float CAN_WALL_SILDE_TIME = 0.3f; // 벽을 탈 수 있는 최소 시간
        private float _inAirTime;

        public PlayerAirState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMover>();
            _attackCompo = entity.GetCompo<PlayerAttackCompo>();
        }

        public override void Enter()
        {
            base.Enter();
            _mover.SetMoveSpeedMultiplier(0.7f);
            _player.InputSO.OnJumpKeyPressed += HandleAirJump;
            _player.InputSO.OnAttackKeyPressed += HandleAirAttack;
            _inAirTime = 0;
        }


        public override void Update()
        {
            base.Update();

            _inAirTime += Time.deltaTime;

            float xInput = _player.InputSO.InputDirection.x;
            if (Mathf.Abs(xInput) > 0)
                _mover.SetMovementX(xInput);

            bool isFall = _mover.YVelocity < 0;

            if (isFall || _inAirTime >= CAN_WALL_SILDE_TIME)
            {
                bool isFrontMove = Mathf.Abs(xInput + _renderer.FacingDirection) > 1;

                if (isFrontMove && _mover.IsWallDetected(_renderer.FacingDirection))
                {
                    _player.ChangeState("WALL_SLIDE");
                }
            }
        }

        public override void Exit()
        {
            _player.InputSO.OnJumpKeyPressed -= HandleAirJump;
            _player.InputSO.OnAttackKeyPressed -= HandleAirAttack;
            _mover.SetMoveSpeedMultiplier(1f);
            base.Exit();
        }

        private void HandleAirAttack()
        {
            if (_attackCompo.CanJumpAttack())
                _player.ChangeState("JUMP_ATTACK");
        }

        private void HandleAirJump()
        {
            if (_player.CanJump)
                _player.ChangeState("JUMP");
        }
    }
}
