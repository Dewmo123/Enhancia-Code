using _00.Work.CDH.Code.Animators;
using _00.Work.CDH.Code.Entities;
using UnityEngine;

namespace _00.Work.CDH.Code.Players.States
{
    public class PlayerSlideState : PlayerGroundState
    {
        private readonly Vector2 _slideOffset = new Vector2(0f, 0.425f);
        private readonly Vector2 _slideSize = new Vector2(0.65f, 0.75f);

        public PlayerSlideState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _mover.CanManualMove = false;
            _mover.AddForceToEntity(new Vector2(5 * _renderer.FacingDirection, 0));
            _mover.SetColliderSize(_slideSize, _slideOffset);
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
            _mover.ResetColliderSize();
            base.Exit();
        }
    }
}