using _00.Work.CDH.Code.Animators;
using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Entities.FSM;

namespace _00.Work.CDH.Code.Players.States
{
    public abstract class PlayerGroundState : EntityState
    {
        protected Player _player;
        protected EntityMover _mover;

        protected static bool _onGround;

        protected PlayerGroundState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMover>();
        }

        public override void Enter()
        {
            base.Enter();
            _player.InputSO.OnJumpKeyPressed += HandleJumpKeyPressed;
            _player.InputSO.OnAttackKeyPressed += HandleAttackKeyPressed;

            _onGround = true;

            _player.ResetJumpCount();
        }

        public override void Update()
        {
            base.Update();
            //범인
            if (_mover.IsGroundDetected() == false && _mover.CanManualMove && !_mover.IsWallDetected(_renderer.FacingDirection))
            {
                _player.ChangeState("FALL");
            }
        }

        public override void Exit()
        {
            _player.InputSO.OnJumpKeyPressed -= HandleJumpKeyPressed;
            _player.InputSO.OnAttackKeyPressed -= HandleAttackKeyPressed;
            base.Exit();
        }

        protected virtual void HandleAttackKeyPressed()
        {
            if (_mover.IsGroundDetected() && _mover.CanManualMove)
                _player.ChangeState("ATTACK");
        }

        protected void HandleJumpKeyPressed()
        {
            if (_player.CanJump)
                _player.ChangeState("JUMP");
        }
    }
}
