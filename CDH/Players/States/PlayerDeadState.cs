using _00.Work.CDH.Code.Animators;
using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Players.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._00.Work.CDH.Code.Players.States
{
    public class PlayerDeadState : PlayerGroundState
    {
        public PlayerDeadState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _mover.CanManualMove = false;
            _mover.StopImmediately(true);
        }
    }
}
