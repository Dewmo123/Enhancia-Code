using _00.Work.CDH.Code.Animators;
using _00.Work.CDH.Code.Entities;

namespace _00.Work.CDH.Code.Players.States
{
    public class PlayerFallState : PlayerAirState
    {
        public PlayerFallState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
        }

        public override void Update()
        {
            base.Update();
            if (_mover.IsGroundDetected())
            {
                _player.ChangeState("IDLE");
            }
        }

    }
}